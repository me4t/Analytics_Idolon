using Code.Data;
using Code.EventBufferService;
using Code.Extensions;
using Code.SaveLoadEventService;
using Code.ServerSender;

namespace Code.Analytics
{
  public class AnalyticsService : IAnalyticsService
  {
    private readonly ISaveLoadEventService _saveLoadService;
    private readonly IEventBuffer _eventBuffer;
    private readonly IServerEventSender _serverEventSender;

    public AnalyticsService(ISaveLoadEventService saveLoadService, IEventBuffer eventBuffer, IServerEventSender serverEventSender)
    {
      _saveLoadService = saveLoadService;
      _eventBuffer = eventBuffer;
      _serverEventSender = serverEventSender;
    }

    public void Initialize()
    {
      PushToBufferEvents();
      SubscribeOnComplete();
    }

    private void SubscribeOnComplete()
    {
      _serverEventSender.OnEventsSent += OnEventsSent;
    }

    public void DisposeSubscribes()
    {
      _serverEventSender.OnEventsSent -= OnEventsSent;
    }

    private void PushToBufferEvents()
    {
      EventList eventList = _saveLoadService.Load();
      foreach (var eventData in eventList.events)
        _eventBuffer.Add(eventData);
    }

    public void OnEventsSent(EventList eventList)
    {
      RemoveFromBackUp(eventList);
      RemoveEventsFromBuffer(eventList);
    }

    private void RemoveEventsFromBuffer(EventList eventList)
    {
      _eventBuffer.RemoveDuplicates(eventList.events);
    }

    private void RemoveFromBackUp(EventList eventList)
    {
      EventList cash = _saveLoadService.Load();
      cash.events.RemoveAll(eventList.events.Contains);
      MakeBackUp(cash);
    }

    private void MakeBackUp(EventList cash)
    {
      _saveLoadService.Save(cash);
    }

    public void TrackEvent(string type, string data)
    {
      _eventBuffer.Add(new EventData(type, data));
      MakeBackUp(_eventBuffer.Events.ToEventList()); 
    }
  }
}