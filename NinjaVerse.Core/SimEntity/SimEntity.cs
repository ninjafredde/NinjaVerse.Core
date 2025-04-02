using System.Collections.Generic;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Timeline;

namespace NinjaVerse.Core
{
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
