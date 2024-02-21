using Microsoft.EntityFrameworkCore;

using Musdis.Common.GrpcProtos;
using Musdis.MusicService.Data;
using Musdis.MusicService.Endpoints;
using Musdis.MusicService.Extensions;
using Musdis.MusicService.Services.Exceptions;
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
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddExceptionHandler<DeveloperExceptionHandler>();
}
else
{
    builder.Services.AddExceptionHandler<ExceptionHandler>();
}
builder.Services.AddProblemDetails();


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("/artists").MapArtists();
app.MapGroup("/artist-types").MapArtistTypes();
app.MapGroup("/releases").MapReleases();
app.MapGroup("/release-types").MapReleaseTypes();
app.MapGroup("/tags").MapTags();
app.MapGroup("/tracks").MapTracks();

app.Run();
