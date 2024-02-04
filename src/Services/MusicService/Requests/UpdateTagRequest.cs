namespace Musdis.MusicService.Requests;

/// <summary>
///     Represents the request to update the <see cref="Models.Tag"/>.
/// </summary>
/// 
/// <param name="Name">
///     The new name to set to the <see cref="Models.Tag"/>.
/// </param>
public sealed record UpdateTagRequest(string Name);
