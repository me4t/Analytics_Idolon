using System;
using System.Collections.Generic;
using Code.Data;
using Code.EventBufferService;
using UnityEngine;
using UnityEngine.Networking;

namespace Code.ServerSender
{
  public interface IWebRequestSender
  {
    UnityWebRequest SendWebRequest(string url, string json);
  }

  public class WebRequestSender : IWebRequestSender
  {
    public UnityWebRequest SendWebRequest(string url, string json)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer()
        };
        www.SetRequestHeader("Content-Type", "application/json");
  
        return www;
    }
  }

  public class ServerEventSender : IServerEventSender
  {
    private readonly IEventBuffer _eventBuffer;
    private readonly IWebRequestSender _webRequestSender;
    private string _serverUrl;
    private float _sendCooldown;
    private bool _isSending;
    private float _cooldownBeforeSend;
    public event Action<EventList> OnEventsSent;

    public string URL
    {
      set => _serverUrl = value;
    }

    public ServerEventSender(IEventBuffer eventBuffer,IWebRequestSender webRequestSender)
    {
      _eventBuffer = eventBuffer;
      _webRequestSender = webRequestSender;
    }


    public void Tick()
    {
      if (_sendCooldown > 0)
      {
        _sendCooldown -= Time.unscaledTime;
      }
      else if (_eventBuffer.IsNotEmpty && !_isSending)
      {
        _sendCooldown = _cooldownBeforeSend;
        SendEvents();
      }
    }


    private void SendEvents()
    {
      _isSending = true;

      EventList eventList = new EventList { events = new List<EventData>(_eventBuffer.Events) };
      string json = JsonUtility.ToJson(eventList);
      UnityWebRequest unityWebRequest = _webRequestSender.SendWebRequest(_serverUrl, json);

      unityWebRequest.SendWebRequest().completed += (_) =>
      {
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
          OnEventsSent?.Invoke(eventList);
        }

        _isSending = false;
      };
    }
  }
}