namespace Musdis.MusicService.Requests;

/// <summary>
///     The request to create an <see cref="Models.ArtistType"/>.
/// </summary>
/// 
/// <param name="Name">
///     The name of the <see cref="Models.ArtistType"/>.
/// </param>
public sealed record CreateArtistTypeRequest(string Name);
