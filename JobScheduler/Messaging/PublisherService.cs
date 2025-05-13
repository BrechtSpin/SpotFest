using MassTransit;

namespace JobScheduler.Messaging;

public partial class PublisherService(IPublishEndpoint publish) : IPublisherService
{
    private readonly IPublishEndpoint _publish = publish;
}
