using Microsoft.EntityFrameworkCore;
using ArtistService.Messaging;
using ArtistService.Repositories;
using ArtistService.Models;
using Contracts;

namespace ArtistService.Services;

public class ArtistServices(ArtistServiceContext context, IPublisherService publisherService) : IArtistServices
{
    private readonly ArtistServiceContext _context = context;
    private readonly IPublisherService _publisherService = publisherService;

    public async Task ArtistMetricStoreInDB(Contracts.ArtistMetric artistMetric)
    {
        await _context.ArtistMetrics.AddAsync(new Models.ArtistMetric
        {
            Guid = artistMetric.Guid,
            ArtistGuid = artistMetric.ArtistGuid,
            Followers = artistMetric.Followers,
            Listeners = artistMetric.Listeners,
            Popularity = artistMetric.Popularity,
            Date = artistMetric.Date,
        });
        await _context.SaveChangesAsync();

    }

    public async Task ResolveHappeningArtist(HappeningArtistIncomplete incompleteHappeningArtist)
    {
        var artist = await _context.Artists
                .FirstOrDefaultAsync(a => a.SpotifyId == incompleteHappeningArtist.SpotifyId);
        if (artist is null)
        {
            artist = new Artist
            {
                SpotifyId = incompleteHappeningArtist.SpotifyId,
                Name = incompleteHappeningArtist.Name
            };
            _context.Artists.Add(artist);
            _context.SaveChanges();
            await _publisherService.ArtistMetricTaskPublisher(new ArtistIdMap
            {
                ArtistGuid = artist.Guid,
                SpotifyId = artist.SpotifyId
            });
        }

        await _publisherService.HappeningArtistCompletePublisher(new HappeningArtistComplete
        {
            HappeningGuid = incompleteHappeningArtist.HappeningGuid,
            ArtistGuid = artist.Guid
        });
    }
}
