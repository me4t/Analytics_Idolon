using Code.Analytics;
using Code.EventBufferService;
using Code.SaveLoadEventService;
using Code.ServerSender;
using UnityEngine;

namespace Code
{
  public class UserExample : MonoBehaviour
  {
    private IServerEventSender _serverSender;
    private IAnalyticsService _analyticsService;
    private IEventBuffer _buffer;
    private ISaveLoadEventService _saveLoadService;

    private void Awake() //Prepare
    {
      _buffer = new EventBuffer();
      _saveLoadService = new SaveLoadEventsService();
      _serverSender = new ServerEventSender(_buffer);
      _serverSender.URL = "Your/url";
      _analyticsService = new AnalyticsService(_saveLoadService, _buffer,_serverSender);
    }

    private void Start() // Load unsent events 
    {
      _analyticsService.Initialize();
    }

    private void Update() //Runtime
    {
      _serverSender.Tick();
    }

    private void OnDestroy()
    {
      _analyticsService.DisposeSubscribes();
    }
  }
}