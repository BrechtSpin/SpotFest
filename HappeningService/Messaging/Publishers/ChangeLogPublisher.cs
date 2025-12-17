using Contracts;

namespace HappeningService.Messaging;

public partial class PublisherService 
{
    public Task ChangeLogMessagePublisher(ChangeLogMessage message)
    {
        return _publish.Publish(message);
    }
}
