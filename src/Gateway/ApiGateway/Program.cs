using Musdis.ApiGateway.Defaults;
using Musdis.ApiGateway.Services;
using Musdis.ApiGateway.Transforms;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddExceptionHandler<DeveloperExceptionHandler>();
}
else
{
    builder.Services.AddExceptionHandler<ExceptionHandler>();
}
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins("https://localhost:3000")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms<AuthTransformProvider>();

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors();

app.MapReverseProxy(builder =>
{
    builder.Use(async (context, next) =>
    {
        var token = context.Request.Cookies[CookieNames.Jwt];
        if (!string.IsNullOrEmpty(token))
        {
            context.Request.Headers.Append("Authorization", "Bearer " + token);
        }

        await next();
    });
});

app.Run();
