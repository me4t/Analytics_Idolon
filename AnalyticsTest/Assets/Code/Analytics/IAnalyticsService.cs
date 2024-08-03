namespace Code.Analytics
{
  public interface IAnalyticsService
  {
    public void Initialize();
    public void TrackEvent(string type, string data);
    public void DisposeSubscribes();
  }
}