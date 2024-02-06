namespace Musdis.MusicService.Requests;

/// <summary>
///     The request to update the <see cref="Models.ReleaseType"/>.
/// </summary>
/// 
/// <param name="Name">
///     A new name of the <see cref="Models.ReleaseType"/> to update.
/// </param>
public sealed record UpdateReleaseTypeRequest(string Name);