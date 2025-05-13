using DataHarvester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHarvester.SpotifyWeb;

public interface ISpotifyWebApiClient
{
    public Task<SpotifyArtist> GetArtistAsync(string spotifyId);
    public Task<List<SpotifyArtist>> GetArtistsByNameAsync(string artistName, int amount = 5);
}
