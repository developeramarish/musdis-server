namespace MusicService.Models;

// TODO Add documentation
public class Artist
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int ArtistTypeId { get; set; }
    public ArtistType? ArtistType { get; set; }
}
