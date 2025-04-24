namespace Contracts;

public record HappeningArtistIncomplete
{
    public Guid HappeningGuid { get; set; }
    public string SpotifyId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
