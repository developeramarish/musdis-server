using FluentValidation;

using Musdis.MusicService.Data;
using Musdis.MusicService.Requests;
using Musdis.MusicService.Services.Data;
using Musdis.MusicService.Validation;

namespace Musdis.MusicService.Extensions;

/// <summary>
///     An extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds data services from <see cref="Musdis.MusicService.Services.Data"/> namespace.
    /// </summary>
    /// 
    /// <param name="services">
    ///     A service collection to add data services.
    /// </param>
    /// 
    /// <returns>
    ///     Provided <paramref name="services"/> with added data services. 
    /// </returns>
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddTransient<IArtistService, ArtistService>();
        services.AddTransient<IArtistTypeService, ArtistTypeService>();
        services.AddTransient<IReleaseTypeService, ReleaseTypeService>();
        services.AddTransient<ITagService, TagService>();
        services.AddTransient<ITrackService, TrackService>();
        services.AddTransient<IReleaseService, ReleaseService>();

        return services;
    }

    /// <summary>
    ///     Adds <see cref="FluentValidation"/> validators.
    /// </summary>
    /// 
    /// <param name="services">
    ///     A service collection to add validators.
    /// </param>
    /// 
    /// <returns>
    ///     Provided <paramref name="services"/> with added validators. 
    /// </returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddScoped<IValidator<CreateArtistTypeRequest>, CreateArtistTypeRequestValidator>();
        services.AddScoped<IValidator<UpdateArtistTypeRequest>, UpdateArtistTypeRequestValidator>();

        services.AddScoped<IValidator<CreateReleaseTypeRequest>, CreateReleaseTypeRequestValidator>();
        services.AddScoped<IValidator<UpdateReleaseTypeRequest>, UpdateReleaseTypeRequestValidator>();

        services.AddScoped<IValidator<CreateTagRequest>, CreateTagRequestValidator>();
        services.AddScoped<IValidator<UpdateTagRequest>, UpdateTagRequestValidator>();

        services.AddScoped<IValidator<CreateTrackRequest>>(sp =>
            new CreateTrackRequestValidator(sp.GetRequiredService<MusicServiceDbContext>())
        );
        services.AddScoped<IValidator<UpdateTrackRequest>>(sp =>
            new UpdateTrackRequestValidator(sp.GetRequiredService<MusicServiceDbContext>())
        );

        services.AddScoped<IValidator<CreateArtistRequest>>(sp =>
            new CreateArtistRequestValidator(sp.GetRequiredService<MusicServiceDbContext>())
        );
        services.AddScoped<IValidator<UpdateArtistRequest>>(sp =>
            new UpdateArtistRequestValidator(sp.GetRequiredService<MusicServiceDbContext>())
        );

        services.AddScoped<IValidator<CreateReleaseRequest>>(sp =>
            new CreateReleaseRequestValidator(sp.GetRequiredService<MusicServiceDbContext>())
        );
        services.AddScoped<IValidator<UpdateReleaseRequest>>(sp =>
            new UpdateReleaseRequestValidator(sp.GetRequiredService<MusicServiceDbContext>())
        );

        services.AddScoped<IValidator<CreateReleaseRequest.TrackInfo>>(sp =>
            new CreateReleaseRequestTrackInfoValidator(sp.GetRequiredService<MusicServiceDbContext>())
        );

        return services;
    }
}