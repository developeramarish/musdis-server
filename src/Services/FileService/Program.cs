using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

using MassTransit;

using Google.Cloud.Storage.V1;

using FluentValidation;

using Musdis.OperationResults;
using Musdis.ResponseHelpers.Extensions;
using Musdis.AuthHelpers.Extensions;
using Musdis.FileService.Options;
using Musdis.FileService.Data;
using Musdis.FileService.Endpoints;
using Musdis.FileService.Services.Storage;
using Musdis.FileService.Services.Exceptions;
using Musdis.FileService.Swagger;
using Musdis.FileService.Validation;
using Musdis.FileService.MessageBroker.Consumers;

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

    busConfigurator.AddDelayedMessageScheduler();

    busConfigurator.AddConsumer<FileUsedConsumer>();
    busConfigurator.AddConsumer<EntityWithFileDeletedConsumer>();
    busConfigurator.AddConsumer<DeleteFileScheduledConsumer>();

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

        config.UseDelayedMessageScheduler();

        config.ConfigureEndpoints(context);
    });
});

// Authentication and authorization
builder.Services.AddCommonAuthentication(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthorization();

builder.Services.AddDbContext<FileServiceDbContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("FileDbConnection")
        ?? throw new InvalidOperationException("Database connection is missing.");
    options.UseNpgsql(connection);
});
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

// Options
builder.Services.AddOptions<FirebaseOptions>()
    .Bind(builder.Configuration.GetSection(FirebaseOptions.Firebase))
    .ValidateOnStart();
builder.Services.AddOptions<FileDeletionOptions>()
    .Bind(builder.Configuration.GetSection(FileDeletionOptions.FileDeletionSettings))
    .ValidateOnStart();

// Environment variables
Environment.SetEnvironmentVariable(
    builder.Configuration["Firebase:KeyEnvironmentVariableName"]
        ?? throw new InvalidOperationException("Cannot start app, configuration is missing Firebase:KeyEnvironmentVariableName"),
    builder.Configuration["Firebase:KeyPath"]
        ?? throw new InvalidOperationException("Cannot start app, configuration is missing Firebase:KeyPath")
);
ValidatorOptions.Global.LanguageManager.Enabled = false;
builder.Services.AddScoped<IValidator<IFormFile>, FormFileValidator>();

builder.Services.AddTransient(_ => StorageClient.Create());
builder.Services.AddTransient<IStorageProvider, FirebaseStorageProvider>();
builder.Services.AddTransient<IStorageService, StorageService>();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FileService", Version = "v1" });
    options.OperationFilter<XsrfHeaderFilter>();
    options.CustomSchemaIds(type => type.ToString());
    options.AddJwtAuthorization();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapGet("antiforgery/token", (
    [FromServices] IAntiforgery antiforgery,
    HttpContext context
) =>
{
    var tokens = antiforgery.GetAndStoreTokens(context);
    if (tokens.RequestToken is null)
    {
        return new Error(
            "Unable to generate antiforgery token, request token is missing."
        ).ToHttpResult(context.Request.Path);
    }
    context.Response.Cookies.Append(
        "XSRF-TOKEN",
        tokens.RequestToken,
        new CookieOptions { HttpOnly = false }
    );

    return Results.Ok();
}).RequireAuthorization();

app.MapGroup("files").MapFiles();

app.UseAntiforgery();


app.Run();
