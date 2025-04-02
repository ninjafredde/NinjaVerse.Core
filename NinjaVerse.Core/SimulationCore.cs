using System;
using System.Collections.Generic;

namespace NinjaVerse.Core
{
    public enum SimType { Universe, Galaxy, StarSystem, Planet, Civilization, NPC }
    public enum GovernmentType { Democracy, Monarchy, Dictatorship, Technocracy, Anarchy }
    public enum CivilizationStage { Primitive, Agrarian, Industrial, Information, Spacefaring, PostSingularity }
    public enum SocialClass { Working, Noble, Religious, Military, Outcast }
    public enum CosmicFate { Unknown, BigFreeze, BigCrunch, BigRip, HeatDeath }

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

    public class SimEntity
    {
        public SimType Type;
        public int Seed;
        public Dictionary<string, object> Traits = new Dictionary<string, object>();
        public TimelineData Timeline = new TimelineData();

        public SimEntity(SimType type, int seed)
        {
            Type = type;
            Seed = seed;
        }
    }

    
}
