using Code.Data;
using NUnit.Framework;

namespace Editor.Tests
{
  [TestFixture]
  public class EventDataTests
  {
    [Test]
    public void Equals_ReturnsTrue_ForSameTypeAndData()
    {
      var eventData1 = new EventData("type", "data");
      var eventData2 = new EventData("type", "data");

      Assert.AreEqual(eventData1, eventData2);
    }

    [Test]
    public void Equals_ReturnsFalse_ForDifferentType()
    {
      var eventData1 = new EventData("type1", "data");
      var eventData2 = new EventData("type2", "data");

      Assert.AreNotEqual(eventData1, eventData2);
    }

    [Test]
    public void Equals_ReturnsFalse_ForDifferentData()
    {
      var eventData1 = new EventData("type", "data1");
      var eventData2 = new EventData("type", "data2");

      Assert.AreNotEqual(eventData1, eventData2);
    }

    [Test]
    public void GetHashCode_SameForSameTypeAndData()
    {
      var eventData1 = new EventData("type", "data");
      var eventData2 = new EventData("type", "data");

      Assert.AreEqual(eventData1.GetHashCode(), eventData2.GetHashCode());
    }

    [Test]
    public void GetHashCode_DifferentForDifferentType()
    {
      var eventData1 = new EventData("type1", "data");
      var eventData2 = new EventData("type2", "data");

      Assert.AreNotEqual(eventData1.GetHashCode(), eventData2.GetHashCode());
    }

    [Test]
    public void GetHashCode_DifferentForDifferentData()
    {
      var eventData1 = new EventData("type", "data1");
      var eventData2 = new EventData("type", "data2");

      Assert.AreNotEqual(eventData1.GetHashCode(), eventData2.GetHashCode());
    }
  }
}