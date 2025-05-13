using Contracts;

namespace TaskScheduler.Messaging;

public partial class PublisherService
{
    public async Task ArtistMetricDataTaskPublisher(SchedulerJob schedulerJob)
    {
        await _publish.Publish(schedulerJob);
    }
}
