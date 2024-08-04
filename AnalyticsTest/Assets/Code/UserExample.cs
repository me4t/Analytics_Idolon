using Code.Analytics;
using Code.CoroutineRunner;
using Code.EventBufferService;
using Code.SaveLoadEventService;
using Code.ServerSender;
using Code.ServerSender.WebRequestSender;
using UnityEngine;

namespace Code
{
  public class UserExample : MonoBehaviour,ICoroutineRunner
  {
    private IServerEventSender _serverSender;
    private IAnalyticsService _analyticsService;
    private IEventBuffer _buffer;
    private ISaveLoadEventService _saveLoadService;

    private void Awake() //Prepare
    {
      _buffer = new EventBuffer();
      _saveLoadService = new SaveLoadEventsService();
      WebRequestSender webSender = new WebRequestSender(); 
      _serverSender = new ServerEventSender(_buffer,webSender,this);
      _serverSender.URL = "Your/url";
      _analyticsService = new AnalyticsService(_saveLoadService, _buffer,_serverSender);
    }

    private void Start() // Load unsent events 
    {
      _analyticsService.Initialize();
    }

    private void Update()
    {
      if (Input.GetKeyUp(KeyCode.L))
        _analyticsService.TrackEvent("levelStart", "1:Started");
      if (Input.GetKeyUp(KeyCode.C)) 
        _analyticsService.TrackEvent("getCoin", "30:GetCoins");
    }

    private void OnDestroy()
    {
      _analyticsService.DisposeSubscribes();
    }
  }
}