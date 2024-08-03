using System;

namespace Code.Data
{
  [Serializable]
  public class EventData
  {
    public string type;
    public string data;

    public EventData(string type, string data)
    {
      this.type = type;
      this.data = data;
    }
    public override bool Equals(object obj)
    {
      if (obj is EventData other)
      {
        return type == other.type && data == other.data;
      }

      return false;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(type, data);
    }
  }
}