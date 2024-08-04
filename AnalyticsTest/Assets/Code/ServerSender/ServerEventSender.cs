using System;
using System.Collections;
using System.Collections.Generic;
using Code.CoroutineRunner;
using Code.Data;
using Code.EventBufferService;
using Code.ServerSender.WebRequestSender;
using UnityEngine;
using UnityEngine.Networking;

namespace Code.ServerSender
{
  public class ServerEventSender : IServerEventSender
  {
    public const float CooldownBeforeSend = 3;
    
    private readonly IEventBuffer _eventBuffer;
    private readonly IWebRequestSender _webRequestSender;
    private readonly ICoroutineRunner _coroutineRunner;
    private string _serverUrl;
    private float _sendCooldown;
    private Coroutine _sendingCoroutine;

    public event Action<EventList> OnEventsSent;

    public string URL
    {
      set => _serverUrl = value;
    }

    public ServerEventSender(IEventBuffer eventBuffer, IWebRequestSender webRequestSender, ICoroutineRunner coroutineRunner)
    {
      _eventBuffer = eventBuffer;
      _webRequestSender = webRequestSender;
      _coroutineRunner = coroutineRunner;
    }

    public void SendBufferedEvents(bool withDelay = true)
    {
      if (withDelay == false)
      {
        SendEventsFromBuffer();
      }
      else if (_sendingCoroutine == null)
      {
        IEnumerator sendCoroutine = SendWithDelay(CooldownBeforeSend);
        _sendingCoroutine = _coroutineRunner.StartCoroutine(sendCoroutine);
      }
    }

    private IEnumerator SendWithDelay(float cooldownBeforeSend)
    {
      yield return new WaitForSecondsRealtime(cooldownBeforeSend);
      SendEventsFromBuffer();
      _sendingCoroutine = null;
    }

    private void SendEventsFromBuffer()
    {
      EventList eventList = new EventList { events = new List<EventData>(_eventBuffer.Events) };
      string json = JsonUtility.ToJson(eventList);
      UnityWebRequest unityWebRequest = _webRequestSender.SendWebRequest(_serverUrl, json);

      unityWebRequest.SendWebRequest().completed += (_) =>
      {
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
          OnEventsSent?.Invoke(eventList);
        }
      };
    }
  }
}