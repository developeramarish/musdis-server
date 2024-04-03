using Musdis.FileService.Options;
using Musdis.FileService.Services.Storage;

using Google.Cloud.Storage.V1;
using Musdis.FileService.Data;
using Microsoft.EntityFrameworkCore;
using Musdis.FileService.Endpoints;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Musdis.FileService.Swagger;
using Musdis.OperationResults;
using Musdis.ResponseHelpers.Extensions;
using Musdis.AuthHelpers.Extensions;
using FluentValidation;
using Musdis.FileService.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommonAuthentication(
    builder.Configuration,
    "JwtSettings"
);
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
    HttpContext context,
    [FromServices] IAntiforgery antiforgery
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
