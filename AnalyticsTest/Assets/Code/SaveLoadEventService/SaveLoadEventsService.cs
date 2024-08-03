using System;
using System.IO;
using Code.Data;
using UnityEngine;

namespace Code.SaveLoadEventService
{
  public class SaveLoadEventsService : ISaveLoadEventService
  {
    private string _eventFilePath;

    public void Save(EventList eventList)
    {
      try
      {
        string json = JsonUtility.ToJson(eventList);
        File.WriteAllText(_eventFilePath, json);
      }
      catch (Exception ex)
      {
        Debug.LogError("Failed to save events: " + ex.Message);
      }
    }

    public EventList Load()
    {
      _eventFilePath = Path.Combine(Application.persistentDataPath, "events.json");
            
      EventList eventLists = new EventList();
            
      try
      {
        if (File.Exists(_eventFilePath))
        {
          string json = File.ReadAllText(_eventFilePath);
          eventLists = JsonUtility.FromJson<EventList>(json);
        }
      }
      catch (Exception ex)
      {
        Debug.LogError("Failed to load events: " + ex.Message);
      }

      return eventLists;
    }
  }
}