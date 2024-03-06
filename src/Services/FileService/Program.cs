using Musdis.FileService.Options;
using Musdis.FileService.Services.Storage;

using Google.Cloud.Storage.V1;
using Musdis.FileService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FileServiceDbContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("FileDbConnection")
        ?? throw new InvalidOperationException("Database connection is missing.");
    options.UseNpgsql(connection);
});

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

builder.Services.AddTransient(_ => StorageClient.Create());
builder.Services.AddTransient<IStorageProvider, FirebaseStorageProvider>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
