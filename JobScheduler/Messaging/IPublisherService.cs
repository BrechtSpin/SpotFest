using Contracts;

namespace JobScheduler.Messaging;

public interface IPublisherService
{
    public Task ArtistMetricDataTaskPublisher(SchedulerJob schedulerJob);
}
