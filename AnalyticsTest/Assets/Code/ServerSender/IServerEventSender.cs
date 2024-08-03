using System;
using Code.Data;

namespace Code.ServerSender
{
  public interface IServerEventSender
  {
    public string URL { set; }
    public void Tick();

    public event Action<EventList> OnEventsSent;
  }
}