//using MassTransit;

//using Contracts;
//using Microsoft.Extensions.DependencyInjection;
//using System.Reflection;
//using DataHarvester.Services;


//namespace DataHarvester.Messaging.Consumers;

//public class SchedulerTaskConsumer : IConsumer<SchedulerTask>, IConsumer<ArtistIdMap>
//{
//    private readonly ITaskHandler _TaskHandler;

//    public SchedulerTaskConsumer(ITaskHandler taskHandler)
//    {
//        _TaskHandler = taskHandler;
//    }

//    public async Task Consume(ConsumeContext<SchedulerTask> context)
//    {
//        Console.WriteLine("schedulerTask");
//        await context.Publish(new ArtistIdMapResolve() { Mode = context.Message.Mode });
//    }

//    public async Task Consume(ConsumeContext<ArtistIdMap> context)
//    {
//        Console.WriteLine("ArtistIdMap");
//        await context.Publish(_TaskHandler.MakeScrapeTask(context.Message));
//    }
//}
