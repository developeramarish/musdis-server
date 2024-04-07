using FluentValidation;

using Musdis.AuthHelpers.Extensions;

using Musdis.IdentityService.Data;
using Musdis.IdentityService.Endpoints;
using Musdis.IdentityService.Requests;
using Musdis.IdentityService.Models;
using Musdis.IdentityService.Options;
using Musdis.IdentityService.Services.Exceptions;
using Musdis.IdentityService.Services.Authentication;
using Musdis.IdentityService.Services.Jwt;
using Musdis.IdentityService.Services.Grpc;
using Musdis.IdentityService.Validation;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Musdis.IdentityService.Authorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var connection = configuration.GetConnectionString("IdentityDbConnection");

// Exception handler
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddExceptionHandler<DeveloperExceptionHandler>();
}
else
{
    builder.Services.AddExceptionHandler<ExceptionHandler>();
}
builder.Services.AddProblemDetails();

// Options

builder.Services.AddOptions<JwtConfigurationOptions>()
    .Bind(builder.Configuration.GetSection(JwtConfigurationOptions.Jwt))
    .ValidateOnStart();
builder.Services.AddOptions<IdentityConfigOptions>()
    .Bind(builder.Configuration.GetSection(IdentityConfigOptions.Identity))
    .ValidateOnStart();

// Database
builder.Services.AddDbContext<IdentityServiceDbContext>(
    options => options.UseNpgsql(connection)
);

// Identity
builder.Services
    .AddIdentityCore<User>(options =>
    {
        var config = builder.Configuration
            .GetSection(IdentityConfigOptions.Identity)
            .Get<IdentityConfigOptions>()
            ?? throw new InvalidOperationException("An Identity configuration section is missing.");

        options.User.AllowedUserNameCharacters = config.User.AllowedUserNameCharacters;
        options.User.RequireUniqueEmail = config.User.RequireUniqueEmail;

        options.Password.RequireDigit = config.Password.RequireDigit;
        options.Password.RequireNonAlphanumeric = config.Password.RequireNonAlphanumeric;
        options.Password.RequireLowercase = config.Password.RequireLowercase;
        options.Password.RequireUppercase = config.Password.RequireUppercase;
        options.Password.RequiredLength = config.Password.RequiredLength;
        options.Password.RequiredUniqueChars = config.Password.RequiredUniqueChars;
    })
    .AddSignInManager<SignInManager<User>>()
    .AddEntityFrameworkStores<IdentityServiceDbContext>();

// Authentication and authorization
builder.Services.AddCommonAuthentication(builder.Configuration.GetSection("Jwt:Security"));
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthorizationPolicies.Admin, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new AdminRequirement());
    });
builder.Services.AddSingleton<IAuthorizationHandler, AdminAuthorizationHandler>();


// Other
builder.Services.AddGrpc();

// Validation
ValidatorOptions.Global.LanguageManager.Enabled = false;
builder.Services.AddTransient<IValidator<SignUpRequest>>(sp => new SignUpRequestValidator(
    sp.GetRequiredService<UserManager<User>>(),
    sp.GetRequiredService<IOptions<IdentityConfigOptions>>()
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

app.MapGroup("/").MapAuthentication();
app.MapGroup("/users")
    .MapUsers();

app.Run();
