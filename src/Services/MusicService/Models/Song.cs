namespace MusicService.Models;

// TODO Add documentation
public class Song
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int ReleaseId { get; set; }
    public Release? Release { get; set; }
    public required int GenreId { get; set; }
    public Genre? Genre { get; set; }
    public required int ArtistId { get; set; }
    public Artist? Artist { get; set; }
}