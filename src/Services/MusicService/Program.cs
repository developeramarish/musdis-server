using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;
using Musdis.MusicService.Endpoints;
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

builder.Services.AddValidators();
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

app.MapGroup("/artists").MapArtists();

app.Run();

return;