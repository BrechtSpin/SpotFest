using Contracts;

namespace DataHarvester.Messaging;

public interface IPublisherService
{
    public Task ArtistMetricDataResponsePublisher(ArtistMetric artistMetric);
}
