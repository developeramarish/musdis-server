using System.Text;

using FluentValidation;

using Musdis.IdentityService.Data;
using Musdis.IdentityService.Requests;
using Musdis.IdentityService.Models;
using Musdis.IdentityService.Options;
using Musdis.IdentityService.Services.Exceptions;
using Musdis.IdentityService.Services.Authentication;
using Musdis.IdentityService.Services.Jwt;
using Musdis.IdentityService.Validation;
using Musdis.IdentityService.Endpoints;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Musdis.IdentityService.Services.Grpc;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var connection = configuration.GetConnectionString("IdentityDbConnection");

// Exception handler
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

// Options
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.Jwt)
);
builder.Services.Configure<IdentityPasswordOptions>(
    builder.Configuration.GetSection(IdentityPasswordOptions.Password)
);

// Database
builder.Services.AddDbContext<IdentityServiceDbContext>(
    options => options.UseNpgsql(connection)
);

// Identity
builder.Services
    .AddIdentityCore<User>(options =>
    {
        var passwordOptions = builder.Configuration
            .GetSection(IdentityPasswordOptions.Password)
            .Get<IdentityPasswordOptions>()
            ?? throw new InvalidOperationException("An Identity configuration section is missing.");

        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = passwordOptions.RequireDigit;
        options.Password.RequireNonAlphanumeric = passwordOptions.RequireNonAlphanumeric;
        options.Password.RequireLowercase = passwordOptions.RequireLowercase;
        options.Password.RequireUppercase = passwordOptions.RequireUppercase;
        options.Password.RequiredLength = passwordOptions.RequiredLength;
        options.Password.RequiredUniqueChars = passwordOptions.RequiredUniqueChars;
    })
    .AddSignInManager<SignInManager<User>>()
    .AddEntityFrameworkStores<IdentityServiceDbContext>();

// Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration
            .GetSection(JwtOptions.Jwt)
            .Get<JwtOptions>()
            ?? throw new InvalidOperationException("A JwtOptions configuration section is missing.");

        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Key)
            ),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

// Other
builder.Services.AddGrpc();

// Validation
ValidatorOptions.Global.LanguageManager.Enabled = false;
builder.Services.AddTransient<IValidator<SignUpRequest>>(sp => new SignUpRequestValidator(
    sp.GetRequiredService<UserManager<User>>(),
    sp.GetRequiredService<IOptions<IdentityPasswordOptions>>()
));

// Utils
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddTransient<IJwtGenerator, JwtGenerator>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var filePath = Path.Combine(
        AppContext.BaseDirectory,
        $"{typeof(Program).Assembly.GetName().Name}.xml");
    options.IncludeXmlComments(filePath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<UserGrpcService>();

app.MapGroup("/api/authentication")
    .MapAuthentication();
app.MapGroup("/api/users")
    .MapUsers();

app.Run();
