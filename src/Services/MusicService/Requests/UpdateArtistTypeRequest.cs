namespace Musdis.MusicService.Requests;

/// <summary>
///     The request to update the <see cref="Models.ArtistType"/>.
/// </summary>
/// 
/// <param name="Name">
///     A new name of the <see cref="Models.ArtistType"/> to update.
/// </param>
public sealed record UpdateArtistTypeRequest(string Name);