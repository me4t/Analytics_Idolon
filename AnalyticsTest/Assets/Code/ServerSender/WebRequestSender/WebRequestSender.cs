using UnityEngine.Networking;

namespace Code.ServerSender.WebRequestSender
{
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
}