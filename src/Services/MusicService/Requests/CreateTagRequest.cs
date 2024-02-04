namespace Musdis.MusicService.Requests;

/// <summary>
///     The request to create a <see cref="Models.Tag"/>.
/// </summary>
/// 
/// <param name="Name">
///     The name of the new <see cref="Models.Tag"/>.
/// </param>
public sealed record CreateTagRequest(string Name);