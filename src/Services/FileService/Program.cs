using FileService.Options;
using FileService.Services.StorageService;

using Google.Cloud.Storage.V1;

var builder = WebApplication.CreateBuilder(args);

// Options
builder.Services.Configure<FirebaseOptions>(
    builder.Configuration.GetSection(FirebaseOptions.Firebase)
);

// Environment variables
var firebaseOptions = builder.Configuration
    .GetSection(FirebaseOptions.Firebase)
    .Get<FirebaseOptions>()!;
Environment.SetEnvironmentVariable(
    firebaseOptions.KeyEnvironmentVariableName,
    firebaseOptions.KeyPath
);

builder.Services.AddTransient(_ => StorageClient.Create());
builder.Services.AddTransient<IStorageService, FirebaseStorageService>();


// builder.Services.AddAntiforgery();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseAntiforgery();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
