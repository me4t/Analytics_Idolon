using System.Collections.Generic;
using System.Linq;
using Code.Data;

namespace Code.EventBufferService
{
  public interface IEventBuffer
  {
    public IEnumerable<EventData> Events { get; }
    bool IsNotEmpty => Events.Count() > 0;
    void RemoveDuplicates(List<EventData> eventList);
    void Add(EventData eventData);
  }
}