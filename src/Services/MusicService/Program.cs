using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Musdis.Common.GrpcProtos;
using Musdis.MusicService.Authorization;
using Musdis.MusicService.Authorization.Handlers;
using Musdis.MusicService.Authorization.Requirements;
using Musdis.MusicService.Data;
using Musdis.MusicService.Defaults;
using Musdis.MusicService.Endpoints;
using Musdis.MusicService.Extensions;
using Musdis.MusicService.Options;
using Musdis.MusicService.Services.Exceptions;
using Musdis.MusicService.Services.Grpc;
using Musdis.MusicService.Services.Utils;

using Slugify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MusicServiceDbContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("MusicDbConnection")
        ?? throw new InvalidOperationException("Database connection is missing.");
    options.UseNpgsql(connection);
});

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

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthorizationPolicies.SameAuthor, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new SameAuthorOrAdminRequirement());
    })
    .AddPolicy(AuthorizationPolicies.Admin, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new AdminRequirement());
    });

builder.Services.AddSingleton<IAuthorizationHandler, ArtistAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ReleaseAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, TrackAuthorizationHandler>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<ISlugHelper, SlugHelper>();
builder.Services.AddTransient<ISlugGenerator, SlugGenerator>();

builder.Services.AddValidators();
builder.Services.AddDataServices();

builder.Services.AddGrpcClient<UserService.UserServiceClient>(options =>
{
    var addressString = builder.Configuration["Services:IdentityService:Address"]
        ?? throw new InvalidOperationException("Configuration section is missing: \"Services:IdentityService:Address\".");
    options.Address = new Uri(addressString);
});
builder.Services.AddTransient<IIdentityUserService, IdentityUserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    options.CustomSchemaIds(type => type.ToString());
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter your JWT token.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddExceptionHandler<DeveloperExceptionHandler>();
}
else
{
    builder.Services.AddExceptionHandler<ExceptionHandler>();
}
builder.Services.AddProblemDetails();


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapGroup("/artists").MapArtists();
app.MapGroup("/artist-types").MapArtistTypes();
app.MapGroup("/releases").MapReleases();
app.MapGroup("/release-types").MapReleaseTypes();
app.MapGroup("/tags").MapTags();
app.MapGroup("/tracks").MapTracks();

app.Run();
