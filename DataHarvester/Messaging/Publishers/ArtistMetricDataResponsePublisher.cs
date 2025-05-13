using Contracts;
namespace DataHarvester.Messaging;

public partial class PublisherService
{
    public Task ArtistMetricDataResponsePublisher(ArtistMetric artistMetric)
    {
        return _publish.Publish(artistMetric);
    }
}