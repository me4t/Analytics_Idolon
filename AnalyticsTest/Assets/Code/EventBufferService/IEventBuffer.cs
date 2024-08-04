using System.Collections.Generic;
using Code.Data;

namespace Code.EventBufferService
{
  public interface IEventBuffer
  {
    public IEnumerable<EventData> Events { get; }
    void RemoveDuplicates(List<EventData> eventList);
    void Add(EventData eventData);
  }
}