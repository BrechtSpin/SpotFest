using Contracts;
namespace DataHarvester.Messaging;

public partial class PublisherService
{
    public Task ArtistMetricFetchedDataPublisher(ArtistMetric artistMetric)
    {
        return _publish.Publish(artistMetric);
    }
}