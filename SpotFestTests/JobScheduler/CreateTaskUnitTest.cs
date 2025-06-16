using JobScheduler.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JobScheduler;
using JobScheduler.Data;
using JobScheduler.Models;
using JobScheduler.Messaging;
using Contracts;
using Moq;

namespace SpotFestTests.JobScheduler
{
    [TestClass]
    public class CreateTaskUnitTests
    {
        private JobSchedulerContext _jobSchedulerContext = null!;
        private Mock<IPublisherService> _mockPublisher = null!;
        private SchedulerJob? _publishedJob = null;
        private JobSchedulerWorker _jobSchedulerWorker = null!;

        private class TestScopeFactory : IServiceScopeFactory
        {
            private readonly JobSchedulerContext _jobSchedulerContext;
            private readonly IPublisherService _mockPublisher;

            public TestScopeFactory(JobSchedulerContext jobSchedulerContext, IPublisherService mockPublisher)
            {
                _jobSchedulerContext = jobSchedulerContext;
                _mockPublisher = mockPublisher;
            }

            public IServiceScope CreateScope() => new TestScope(_jobSchedulerContext, _mockPublisher);

            private class TestScope : IServiceScope
            {
                public IServiceProvider ServiceProvider { get; }

                public TestScope(JobSchedulerContext jobSchedulerContext, IPublisherService mockPublisher)
                {
                    var services = new ServiceCollection()
                        .AddSingleton(jobSchedulerContext)
                        .AddSingleton(mockPublisher)
                        .BuildServiceProvider();
                    ServiceProvider = services;
                }
                public void Dispose()
                {
                    (ServiceProvider as IDisposable)?.Dispose();
                }
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            // in-memory EF DB
            var options = new DbContextOptionsBuilder<JobSchedulerContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .Options;
            _jobSchedulerContext = new JobSchedulerContext(options);

            // mock IPublisher
            _mockPublisher = new Mock<IPublisherService>();
            _mockPublisher
                .Setup(p => p.ArtistMetricDataTaskPublisher(It.IsAny<SchedulerJob>()))
                .Callback<SchedulerJob>(j => _publishedJob = j)
                .Returns(Task.CompletedTask);

            _jobSchedulerWorker = new JobSchedulerWorker(
                NullLogger<JobSchedulerWorker>.Instance,
                new TestScopeFactory(_jobSchedulerContext, _mockPublisher.Object)
            );
        }        

        [TestMethod]
        public async Task CreateTask_WhenDbEmpty_CreateDbEntryAndPublishJob()
        {
            // arrange
            var now = DateTime.UtcNow;

            // act
            await _jobSchedulerWorker.CreateTask(now);

            // assert
            Assert.IsNotNull(_publishedJob, "Expected a published Job");
            Assert.AreEqual("artistmetric", _publishedJob.Type);
            Assert.AreEqual(1, _jobSchedulerContext.SchedulerLogs.Count(), "Expected exactly one log entry");
            Assert.AreEqual(now.Date, _jobSchedulerContext.SchedulerLogs.Last().JobDate.Date);
        }

        [TestMethod]
        public async Task CreateTask_WhenTodayNoLog_CreateDbEntryAndPublishJob()
        {
            // arrange
            _jobSchedulerContext.SchedulerLogs.Add(new JobSchedulerLog { JobDate = DateTime.UtcNow.AddDays(-1) });
            _jobSchedulerContext.SaveChanges();

            var now = DateTime.UtcNow;

            // act
            await _jobSchedulerWorker.CreateTask(DateTime.UtcNow);

            // assert
            Assert.IsNotNull(_publishedJob, "Expected a published Job");
            Assert.AreEqual("artistmetric", _publishedJob.Type);
            Assert.AreEqual(2, _jobSchedulerContext.SchedulerLogs.Count(), "Expected exactly two log entry");
            Assert.AreEqual(now.Date, _jobSchedulerContext.SchedulerLogs.Last().JobDate.Date);
        }

        [TestMethod]
        public async Task CreateTask_WhenTodayLogExists_DoNothing()
        {
            // arrange
            _jobSchedulerContext.SchedulerLogs.Add(new JobSchedulerLog { JobDate = DateTime.UtcNow });
            _jobSchedulerContext.SaveChanges();

            // act
            await _jobSchedulerWorker.CreateTask(DateTime.UtcNow);

            // assert
            Assert.IsNull(_publishedJob, "Expected no published Job");
            Assert.AreEqual(1, _jobSchedulerContext.SchedulerLogs.Count(), "Expected no new log entries");
        }
    }
}
