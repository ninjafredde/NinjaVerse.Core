using System;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Timeline;
namespace NinjaVerse.Core
{
    public class StarSystemTraits
    {
        public int Age = 0;
        public int StarCount = 1;
        public int PlanetCount = 0;
        public int HabitablePlanets = 0;
        public int PlanetSeedsGenerated = 0;
    }

    public class StarSystemSimulator : ITemporalSimulator
    {
        public void Simulate(SimEntity entity, int years)
        {
            if (entity.Type != SimType.StarSystem) return;

            StarSystemTraits traits;
            object raw;
            if (!entity.Traits.TryGetValue("traits", out raw) || !(raw is StarSystemTraits))
            {
                traits = new StarSystemTraits();
                entity.Traits["traits"] = traits;
            }
            else
            {
                traits = (StarSystemTraits)raw;
            }

            Random rng = new Random(entity.Seed);

            for (int y = 0; y < years; y++)
            {
                traits.Age++;

                // Form a planet
                if (traits.PlanetCount < 12 && rng.NextDouble() < 0.2)
                {
                    traits.PlanetCount++;

                    bool habitable = rng.NextDouble() < 0.15;
                    if (habitable)
                        traits.HabitablePlanets++;

                    traits.PlanetSeedsGenerated++;

                    entity.Timeline.Events.Add(new HistoricalEvent(
                        TemporalResolutionEngine.CurrentYear + y,
                        "Planet Formed",
                        habitable ? "A potentially habitable planet formed." : "A rocky or gaseous planet formed."
                    ));
                }
            }
        }
    }
}
