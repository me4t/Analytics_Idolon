using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Code.EventBufferService;
using NUnit.Framework;

namespace Editor.Tests
{
  [TestFixture]
  public class EventBufferTests
  {
    private EventBuffer _eventBuffer;

    [SetUp]
    public void SetUp()
    {
      _eventBuffer = new EventBuffer();
    }

    [Test]
    public void Add_AddsEventDataToList()
    {
      var eventData = new EventData("type", "data");

      _eventBuffer.Add(eventData);

      Assert.AreEqual(1, _eventBuffer.Events.Count());
      Assert.AreEqual(eventData, _eventBuffer.Events.First());
    }

    [Test]
    public void RemoveDuplicates_RemovesDuplicatesFromList()
    {
      var eventData1 = new EventData("type", "data");
      var eventData2 = new EventData("type", "data");
      var eventData3 = new EventData("type2", "data2");

      _eventBuffer.Add(eventData1);
      _eventBuffer.Add(eventData3);

      _eventBuffer.RemoveDuplicates(new List<EventData> { eventData2 });

      var events = _eventBuffer.Events.ToList();
      Assert.AreEqual(1, events.Count);
      Assert.AreEqual(eventData3, events[0]);
    }

    [Test]
    public void RemoveDuplicates_DoesNotRemoveNonDuplicates()
    {
      var eventData1 = new EventData("type", "data1");
      var eventData2 = new EventData("type2", "data2");

      _eventBuffer.Add(eventData1);
      _eventBuffer.Add(eventData2);

      _eventBuffer.RemoveDuplicates(new List<EventData> { new EventData("type3", "data3") });

      var events = _eventBuffer.Events.ToList();
      Assert.AreEqual(2, events.Count);
      Assert.Contains(eventData1, events);
      Assert.Contains(eventData2, events);
    }
  }
}