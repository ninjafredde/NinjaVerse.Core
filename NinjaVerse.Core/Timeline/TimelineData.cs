namespace NinjaVerse.Core.Timeline
{
    using System.Collections.Generic;

    public class HistoricalEvent
    {
        public int Year;
        public string Type;
        public string Description;

        public HistoricalEvent(int year, string type, string description)
        {
            Year = year;
            Type = type;
            Description = description;
        }
    }

    public class TimelineData
    {
        public List<HistoricalEvent> Events = new List<HistoricalEvent>();
    }
}
