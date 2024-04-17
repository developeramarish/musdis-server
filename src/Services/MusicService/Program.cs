using MassTransit;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Musdis.AuthHelpers.Extensions;
using Musdis.Common.GrpcProtos;
using Musdis.MusicService.Authorization;
using Musdis.MusicService.Authorization.Handlers;
using Musdis.MusicService.Authorization.Requirements;
using Musdis.MusicService.Data;
using Musdis.MusicService.Defaults;
using Musdis.MusicService.Endpoints;
using Musdis.MusicService.Extensions;
using Musdis.MusicService.Services.Exceptions;
using Musdis.MusicService.Services.Grpc;
using Musdis.MusicService.Services.Utils;

using Slugify;

var builder = WebApplication.CreateBuilder(args);

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

// Message broker
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((context, config) =>
    {
        config.Host(
            builder.Configuration["MessageBroker:Host"],
            builder.Configuration["MessageBroker:VirtualHost"],
            h =>
            {
                h.Username(builder.Configuration["MessageBroker:Username"]);
                h.Password(builder.Configuration["MessageBroker:Password"]);
            }
        );

        config.ConfigureEndpoints(context);
    });
});

// Database
builder.Services.AddDbContext<MusicServiceDbContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("MusicDbConnection")
        ?? throw new InvalidOperationException("Database connection is missing.");
    options.UseNpgsql(connection);
});

// Authentication and authorization
builder.Services.AddCommonAuthentication(builder.Configuration.GetSection("Jwt"));

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

// Other
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
    options.SwaggerDoc("v1", new() { Title = "MusicService", Version = "v1" });
    options.CustomSchemaIds(type => type.ToString());
    options.AddJwtAuthorization();
});


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
