using System.Collections.Generic;
using System.Linq;
using Code.Data;

namespace Code.Extensions
{
  public static class Extensions
  {
    public static EventList ToEventList(this IEnumerable<EventData> events)
    {
      return new EventList { events = events.ToList() };
    }
    public static EventList Union(this EventList events,EventList newEvents)
    {
      var combinedList = new EventList { events = new List<EventData>(events.events) };
      combinedList.events.AddRange(newEvents.events);
      return combinedList;
    }
  }
}