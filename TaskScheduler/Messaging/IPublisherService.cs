using Contracts;

namespace TaskScheduler.Messaging;

public interface IPublisherService
{
    public Task ArtistMetricDataTaskPublisher(SchedulerTask schedulerTask);
}
