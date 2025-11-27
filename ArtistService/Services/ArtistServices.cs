using Microsoft.EntityFrameworkCore;
using ArtistService.Messaging;
using ArtistService.Models;
using Contracts;
using ArtistService.Data.Repositories;
using ArtistService.DTO;

namespace ArtistService.Services;

public class ArtistServices(ArtistServiceContext context, IPublisherService publisherService) : IArtistServices
{
    private readonly ArtistServiceContext _context = context;
    private readonly IPublisherService _publisherService = publisherService;

    public async Task ArtistMetricDataJob(SchedulerJob Job)
    {
        if (Job.Type == "artistmetric")
        {
            List<ArtistIdMap> artistIdMaps = [];
            if (Job.Mode == "all")
            {
                artistIdMaps = await _context.Artists.Select(x => new ArtistIdMap
                {
                    ArtistGuid = x.Guid,
                    SpotifyId = x.SpotifyId
                }).ToListAsync();
            }

            foreach(var artistIdMap in artistIdMaps)
            {
                await _publisherService.ArtistMetricHarvestRequestPublisher(artistIdMap);
            }
        }
    }

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

    public async Task<ArtistSummary[]> ArtistSummariesFromGuids(Guid[] guids)
    {
        var artists = await _context.Artists
            .Where(a => guids.Contains(a.Guid))
            .ToArrayAsync();

        return Array.ConvertAll(
            artists,
            a => new ArtistSummary
            {
                Guid = a.Guid,
                Name = a.Name,
                PictureSmallUrl = a.PictureSmallUrl,
            });
    }

    public async Task<string?> GetArtistGuidByName(string name)
    {
        var artists = await _context.Artists.Where(a =>  a.Name == name)
            .Include(a => a.Metrics)
            .OrderByDescending(a => a.Metrics.Max(m => m.Listeners))
            .ToListAsync();
        if(artists is null) return null;
        return artists[0].Guid.ToString();
    }

    public async Task<ArtistSummary[]> GetArtistsSearch(string query, int index)
    {
        if (query == null || query == "" || index < 0) return [];

        var artists = await _context.Artists
            .OrderBy(a => a.Name)
            .Where(a => a.Name.StartsWith(query))
            .Skip(index)
            .Take(10)
            .ToArrayAsync();

        return Array.ConvertAll(
            artists,
            a => new ArtistSummary
            {
                Guid = a.Guid,
                Name = a.Name,
                PictureSmallUrl = a.PictureSmallUrl,
            });
    }

    public async Task<ArtistWithMetricsDTO?> GetArtistWithMetrics(Guid guid)
    {
        var artistWithMetrics = await _context.Artists
            .Include(a => a.Metrics)
            .FirstOrDefaultAsync(a => a.Guid == guid);
        if (artistWithMetrics == null) return null;
        return new ArtistWithMetricsDTO
        {
            ArtistGuid = guid,
            Name = artistWithMetrics.Name,
            PictureMediumUrl = artistWithMetrics.PictureMediumUrl,
            ArtistMetrics = artistWithMetrics.Metrics
            .Select(am => new ArtistMetricDTO
            {
                Date = am.Date,
                Followers = am.Followers,
                Popularity = am.Popularity,
                Listeners = am.Listeners
            })
            .OrderBy(am => am.Date)
            .ToArray()
        };
    }

    public async Task ResolveHappeningArtist(HappeningArtistIncomplete incompleteHappeningArtist)
    {
        var artist = await _context.Artists
                .FirstOrDefaultAsync(a => a.SpotifyId == incompleteHappeningArtist.SpotifyId);
        if (artist is null)
        {
            var response = await _publisherService.GetArtistSpotifyAsync(
                new ArtistSpotifyRequest
                {
                    Name = incompleteHappeningArtist.Name,
                    SpotifyId = incompleteHappeningArtist.SpotifyId,
                });
            if (response.SpotifyId == "") return;
            artist = new Artist
            {
                SpotifyId = response.SpotifyId,
                Name = response.Name,
                PictureSmallUrl = response.PictureSmallUrl,
                PictureMediumUrl = response.PictureMediumUrl,
            };
            _context.Artists.Add(artist);
            _context.SaveChanges();
            await _publisherService.ArtistMetricHarvestRequestPublisher( new ArtistIdMap {
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
