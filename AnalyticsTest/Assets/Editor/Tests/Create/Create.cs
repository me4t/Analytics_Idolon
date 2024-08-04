using Code.Analytics;
using Code.EventBufferService;
using Code.SaveLoadEventService;
using Code.ServerSender;
using Code.ServerSender.WebRequestSender;

namespace Editor.Tests.Create
{
  public static class Create
  {
    public static EventBuffer EventBuffer()
    {
      return new EventBuffer();
    }
    public static WebRequestSender WebRequestSender()
    {
      return new WebRequestSender();
    }
    public static SaveLoadEventsService SaveLoadEventService()
    {
      return new SaveLoadEventsService();
    }
    public static AnalyticsService AnalyticsService(IEventBuffer eventBuffer,ISaveLoadEventService saveLoadEventService,IServerEventSender serverEventSender)
    {
      return new AnalyticsService(saveLoadEventService,eventBuffer,serverEventSender);
    }
  }
}