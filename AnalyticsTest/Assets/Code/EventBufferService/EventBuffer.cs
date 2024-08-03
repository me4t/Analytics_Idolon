using System.Collections.Generic;
using Code.Data;

namespace Code.EventBufferService
{
  public class EventBuffer : IEventBuffer
  {
    private readonly List<EventData> _eventList = new List<EventData>();
    public IEnumerable<EventData> Events => _eventList;

    public void RemoveDuplicates(List<EventData> eventList)
    {
      _eventList.RemoveAll(eventList.Contains); // Equals in EventData override 
    }

    public void Add(EventData eventData)
    {
      _eventList.Add(eventData);
    }
  }
}