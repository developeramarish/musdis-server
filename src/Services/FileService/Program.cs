using FileService.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => 
    Environment.GetEnvironmentVariable(firebaseOptions.KeyEnvironmentVariableName)
);

app.Run();