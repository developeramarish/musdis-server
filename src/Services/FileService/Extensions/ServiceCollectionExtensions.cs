using Google.Cloud.Storage.V1;

using Musdis.FileService.Services.Storage;

namespace Musdis.FileService.Extensions;

/// <summary>
///     Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds storage services to the DI container.
    /// </summary>
    /// 
    /// <param name="services">
    ///     The service collection.
    /// </param>
    /// 
    /// <returns>
    ///     The service collection.
    /// </returns>
    public static IServiceCollection AddStorageServices(this IServiceCollection services)
    {
        services.AddTransient(_ => StorageClient.Create());
        services.AddTransient<IStorageProvider, FirebaseStorageProvider>();

        return services;
    }
}