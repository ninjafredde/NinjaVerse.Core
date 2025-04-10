using NinjaVerse.Core.Enums;
using System.Collections.Generic;

namespace NinjaVerse.Core.Models
{
    public class SimEntity
    {
        public SimType Type;
        public int Seed;
        public Dictionary<string, object> Traits = new Dictionary<string, object>();
        public TimelineData Timeline = new TimelineData();

        // Probabilistic metrics
        public float LifeProbability { get; set; } = 0.0f;
        public float CivilizationProbability { get; set; } = 0.0f;

        public SimEntity(SimType type, int seed)
        {
            Type = type;
            Seed = seed;
        }
    }
}
