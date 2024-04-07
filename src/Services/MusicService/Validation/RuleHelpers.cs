using System.Globalization;

using Microsoft.EntityFrameworkCore;

using Musdis.MusicService.Data;

namespace Musdis.MusicService.Validation;

public static class RuleHelpers
{
    public static async Task<bool> BeExistingArtistTypeSlugAsync(
        string artistTypeSlug,
        MusicServiceDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var artist = await dbContext.ArtistTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Slug == artistTypeSlug, cancellationToken);

        return artist is not null;
    }
    public static async Task<bool> BeExistingReleaseIdAsync(
        Guid releaseId,
        MusicServiceDbContext dbContext,
        CancellationToken cancellationToken = default
    )
    {
        var release = await dbContext.Releases.AsNoTracking().FirstOrDefaultAsync(
            x => x.Id == releaseId,
            cancellationToken
        );

        return release is not null;
    }
    public static async Task<bool> BeExistingArtistIdsAsync(
        IEnumerable<Guid> artistIds,
        MusicServiceDbContext dbContext,
        CancellationToken cancellationToken = default
    )
    {
        var existingCount = await dbContext.Artists
            .AsNoTracking()
            .Where(a => artistIds.Contains(a.Id))
            .CountAsync(cancellationToken);

        return existingCount == artistIds.Count();
    }

    public static bool BeDateString(string value)
    {
        return DateTime.TryParse(value, CultureInfo.InvariantCulture, out var _);
    }
    public static async Task<bool> BeExistingReleaseTypeSlugAsync(
        string releaseTypeSlug,
        MusicServiceDbContext dbContext,
        CancellationToken cancellationToken = default
    )
    {
        var release = await dbContext.ReleaseTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Slug == releaseTypeSlug, cancellationToken);

        return release is not null;
    }

    public static async Task<bool> BeExistingTagSlugsAsync(
        IEnumerable<string> tagSlugs,
        MusicServiceDbContext dbContext,
        CancellationToken cancellationToken = default
    )
    {
        var existingCount = await dbContext.Tags
            .AsNoTracking()
            .Where(t => tagSlugs.Contains(t.Slug))
            .CountAsync(cancellationToken);

        return existingCount == tagSlugs.Count();
    }

    public static bool BeValidUrl(string value)
    {
        return Uri.TryCreate(value, UriKind.Absolute, out _);
    }
}