using System;
using System.IO;
using Code.Data;
using UnityEngine;

namespace Code.SaveLoadEventService
{
  public class SaveLoadEventsService : ISaveLoadEventService
  {
    private static string EventFilePath => Path.Combine(Application.persistentDataPath, "events.json");

    public void Save(EventList eventList)
    {
      try
      {
        string json = JsonUtility.ToJson(eventList);
        File.WriteAllText(EventFilePath, json);
      }
      catch (Exception ex)
      {
        Debug.LogError("Failed to save events: " + ex.Message);
      }
    }

    public EventList Load()
    {
      EventList eventLists = new EventList();

      try
      {
        if (File.Exists(EventFilePath))
        {
          string json = File.ReadAllText(EventFilePath);
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