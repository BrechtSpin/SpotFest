using System;
using System.Collections.Generic;
namespace Contracts;

public record ArtistMetric
{
    public Guid Guid { get; set; }
    public Guid ArtistGuid { get; set; }
    public DateTime Date { get; set; }
    //9/3/2026 deprecated fields from spotify. may come back later? unlikely
    //public required int Followers { get; set; }
    //public required int Popularity { get; set; }
    public long Listeners { get; set; }
}
