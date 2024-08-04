using System.Collections.Generic;
using System.Linq;
using Code.Analytics;
using Code.Data;
using Code.EventBufferService;
using Code.SaveLoadEventService;
using Code.ServerSender;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Tests
{
    [TestFixture]
    public class AnalyticsServiceTests
    {
        private ISaveLoadEventService _mockSaveLoadService;
        private IEventBuffer _mockEventBuffer;
        private IServerEventSender _mockServerEventSender;
        private AnalyticsService _analyticsService;

        [SetUp]
        public void SetUp()
        {
            _mockSaveLoadService = Substitute.For<ISaveLoadEventService>();
            _mockEventBuffer = Create.Create.EventBuffer();
            _mockServerEventSender = Substitute.For<IServerEventSender>();

            _analyticsService = new AnalyticsService(
                _mockSaveLoadService,
                _mockEventBuffer,
                _mockServerEventSender);
        }

        [TearDown]
        public void TearDown()
        {
            _analyticsService.DisposeSubscribes();
        }

        [Test]
        public void Initialize_ShouldLoadEventsFromService_AndAddToBuffer()
        {
            // Arrange
            EventData sampleEvent = new EventData("sampleType", "sampleData");
            EventList events = new EventList { events = new List<EventData> { sampleEvent } };
            _mockSaveLoadService.Load().Returns(events);

            // Act
            _analyticsService.Initialize();

            // Assert
            EventList eventList = _mockSaveLoadService.Load();
            IEnumerable<EventData> eventDatas = _mockEventBuffer.Events;
            eventList.events.Count.Should().Be(1);
            eventDatas.Count().Should().Be(1);
            eventDatas.Should().Contain(sampleEvent);
        }
        [Test]
        public void TrackEvent_ShouldAddEventToBuffer_AndSaveToBackup()
        {
            // Arrange
            string eventType = "testType";
            string eventData = "testData";
            SaveLoadEventsService saveLoadService = Create.Create.SaveLoadEventService();
            EventBuffer eventBuffer = Create.Create.EventBuffer();
            IServerEventSender serverEventSender = Substitute.For<IServerEventSender>();

            AnalyticsService analyticsService = new AnalyticsService(
                    saveLoadService,
                    eventBuffer,
                    serverEventSender);
            
            
            // Act
            analyticsService.TrackEvent(eventType, eventData);

            // Assert
            EventList eventList = saveLoadService.Load();
            eventBuffer.Events.Count().Should().Be(1);
            eventList.events.Count.Should().Be(1);
        }

        [Test]
        public void OnEventsSent_ShouldRemoveProcessedEventsFromBuffer_AndUpdateBackup()
        {
            // Arrange
            EventData event1 = new EventData("type", "data");
            EventData event2 = new EventData("type1", "data1");
            EventData event3 = new EventData("type3", "data3");
            EventList eventsToRemove = new EventList { events = new List<EventData> { event1,event2 } };
            
            SaveLoadEventsService saveLoadService = Create.Create.SaveLoadEventService();
            EventBuffer eventBuffer = Create.Create.EventBuffer();
            IServerEventSender serverEventSender = Substitute.For<IServerEventSender>();
            AnalyticsService analyticsService = new AnalyticsService(
                    saveLoadService,
                    eventBuffer,
                    serverEventSender);
                    
            analyticsService.TrackEvent(event2.type,event2.data);
            analyticsService.TrackEvent(event1.type,event1.data);
            analyticsService.TrackEvent(event3.type,event3.data);
            
            // Act
            analyticsService.OnEventsSent(eventsToRemove);

            // Assert
            EventList eventList = saveLoadService.Load();
            eventList.events.Count.Should().Be(1);
            eventList.events.Should().Contain(event3);
        }
    }
}
