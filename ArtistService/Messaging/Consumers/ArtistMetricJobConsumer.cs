using ArtistService.Models;
using ArtistService.Services;
using Contracts;
using MassTransit;

namespace ArtistService.Messaging.Consumers
{
    public class ArtistMetricJobConsumer(IArtistServices artistServices) : IConsumer<SchedulerJob>
    {
        private readonly IArtistServices _artistServices = artistServices;
        public async Task Consume(ConsumeContext<SchedulerJob> context)
        {
            var newJob = context.Message;
            await _artistServices.ArtistMetricDataJob(newJob);
        }
    }
}
