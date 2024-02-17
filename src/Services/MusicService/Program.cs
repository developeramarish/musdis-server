using Microsoft.EntityFrameworkCore;

using Musdis.Common.GrpcProtos;
using Musdis.MusicService.Data;
using Musdis.MusicService.Endpoints;
using Musdis.MusicService.Extensions;
using Musdis.MusicService.Services.Utils;

using Slugify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MusicServiceDbContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("MusicDbConnection")
        ?? throw new InvalidOperationException("Database connection is missing.");
    options.UseNpgsql(connection);
});

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
