using System.Text;

using FluentValidation;

using Musdis.IdentityService.Data;
using Musdis.IdentityService.Models;
using Musdis.IdentityService.Models.Requests;
using Musdis.IdentityService.Options;
using Musdis.IdentityService.Services.AuthenticationService;
using Musdis.IdentityService.Services.JwtGenerator;
using Musdis.IdentityService.Validation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var connection = configuration.GetConnectionString("IdentityDbConnection");

// Options
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.Jwt)
);
builder.Services.Configure<IdentityPasswordOptions>(
    builder.Configuration.GetSection(IdentityPasswordOptions.Password)
);

// Database
builder.Services.AddDbContext<IdentityServiceDbContext>(options =>
    options.UseNpgsql(connection)
);

// Identity
builder.Services.AddIdentityCore<User>(
    options =>
    {
        var passwordOptions = builder.Configuration
            .GetSection(IdentityPasswordOptions.Password)
            .Get<IdentityPasswordOptions>()!;

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
            .Get<JwtOptions>()!;
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

// Validation
builder.Services.AddTransient<IValidator<SignInRequest>, SignInRequestValidator>();
builder.Services.AddTransient<IValidator<SignUpRequest>>(sp =>
    new SignUpRequestValidator(
        sp.GetRequiredService<UserManager<User>>(),
        sp.GetRequiredService<IOptions<IdentityPasswordOptions>>()
    )
);

// Utils
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddTransient<IJwtGenerator, JwtGenerator>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

builder.Services.AddControllers();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
