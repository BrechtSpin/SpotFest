using Contracts;

namespace JobScheduler.Messaging;

public partial class PublisherService
{
    public async Task ArtistMetricDataTaskPublisher(SchedulerJob schedulerJob)
    {
        await _publish.Publish(schedulerJob);
    }
}
