using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Extensions;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services;
using Musdis.MusicService.Services.Data;

using Slugify;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var connection = configuration.GetConnectionString("MusicDbConnection");

builder.Services.AddDbContext<MusicServiceDbContext>(options =>
    options.UseNpgsql(connection)
);

builder.Services.AddTransient<ISlugHelper, SlugHelper>();
builder.Services.AddTransient<ISlugGenerator, SlugGenerator>();

builder.Services.AddDataServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/a", async (MusicServiceDbContext dbContext) =>
{
    return Results.Ok(await dbContext.Artists.ToListAsync());
});

app.MapPost("/artist", async (
    CreateArtistRequest request,
    [FromServices] IArtistService artistService
) =>
{
    var result = await artistService.CreateAsync(request);

    return result.IsSuccess
        ? Results.Ok(result.Value)
        : Results.Ok(result.Error);
});


app.Run();

return;