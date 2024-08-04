using System.IO;
using Code.Data;
using Code.SaveLoadEventService;
using NUnit.Framework;
using UnityEngine;

namespace Editor.Tests
{
  [TestFixture]
  public class SaveLoadEventsServiceTests
  {
    private string _mockEventFilePath;
    private SaveLoadEventsService _saveLoadEventsService;
    private EventList _mockEventList;

    [SetUp]
    public void SetUp()
    {
      _mockEventFilePath = Path.Combine(Application.persistentDataPath, "events.json");

      _saveLoadEventsService = new SaveLoadEventsService();
      _mockEventList = new EventList
      {
          events = new System.Collections.Generic.List<EventData>
          {
              new EventData("type1", "data1"),
              new EventData("type2", "data2")
          }
      };
    }

    [Test]
    public void Save_SavesEventsToFile()
    {
      // Arrange
      string json = JsonUtility.ToJson(_mockEventList);

      // Act
      _saveLoadEventsService.Save(_mockEventList);

      // Assert
      string savedJson = File.ReadAllText(_mockEventFilePath);
      Assert.AreEqual(json, savedJson);
    }

    [Test]
    public void Load_LoadsEventsFromFile()
    {
      // Arrange
      string json = JsonUtility.ToJson(_mockEventList);
      File.WriteAllText(_mockEventFilePath, json);

      // Act
      EventList loadedEvents = _saveLoadEventsService.Load();

      // Assert
      Assert.AreEqual(_mockEventList.events.Count, loadedEvents.events.Count);
      for (int i = 0; i < _mockEventList.events.Count; i++)
      {
        Assert.AreEqual(_mockEventList.events[i].type, loadedEvents.events[i].type);
        Assert.AreEqual(_mockEventList.events[i].data, loadedEvents.events[i].data);
      }
    }

    [Test]
    public void Load_ReturnsEmptyEventList_WhenFileDoesNotExist()
    {
      // Arrange
      if (File.Exists(_mockEventFilePath))
      {
        File.Delete(_mockEventFilePath);
      }

      // Act
      EventList loadedEvents = _saveLoadEventsService.Load();

      // Assert
      Assert.IsEmpty(loadedEvents.events);
    }

    [Test]
    public void Save_DoesNotThrowException_WhenExceptionOccurs()
    {
      // Arrange
      string invalidFilePath = "/invalid/path/events.json";

      // Act & Assert
      Assert.DoesNotThrow(() => { _saveLoadEventsService.Save(new EventList()); });
    }

    [Test]
    public void Load_DoesNotThrowException_WhenExceptionOccurs()
    {
      // Arrange
      string invalidFilePath = "/invalid/path/events.json";

      // Act & Assert
      Assert.DoesNotThrow(() =>
      {
        var result = _saveLoadEventsService.Load();
      });
    }
  }
}