using System;
using System.Collections.Generic;

namespace Code.Data
{
  [Serializable]
  public class EventList
  {
    public List<EventData> events = new List<EventData>();
  }
}