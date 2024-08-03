using Code.Data;

namespace Code.SaveLoadEventService
{
  public interface ISaveLoadEventService
  {
    public void Save(EventList eventList);
    public EventList Load();
  }
}