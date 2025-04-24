using System;
using System.Collections.Generic;
namespace Contracts;

public record ArtistMetric
{
    public Guid Guid { get; set; }
    public Guid ArtistGuid { get; set; }
    public DateTime Date { get; set; }
    public required int Followers { get; set; }
    public required int Popularity { get; set; }
    public long Listeners { get; set; }
}
