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