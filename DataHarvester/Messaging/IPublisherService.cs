using Contracts;

namespace DataHarvester.Messaging;

public interface IPublisherService
{
    public Task ArtistMetricFetchedDataPublisher(ArtistMetric artistMetric);
}
