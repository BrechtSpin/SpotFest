using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHarvester.SpotifyWeb;

public interface ISpotifyWebScraper
{
    public Task<long> GetListenersAsync(string ArtistUid);
}
