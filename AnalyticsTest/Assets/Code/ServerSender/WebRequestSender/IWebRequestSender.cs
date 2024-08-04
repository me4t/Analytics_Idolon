using UnityEngine.Networking;

namespace Code.ServerSender.WebRequestSender
{
  public interface IWebRequestSender
  {
    UnityWebRequest SendWebRequest(string url, string json);
  }
}