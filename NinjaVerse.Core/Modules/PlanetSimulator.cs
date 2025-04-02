using System;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Timeline;

namespace NinjaVerse.Core
{
    public class PlanetTraits
    {
        public int Age = 0;
        public float Temperature;   // °C
        public float Gravity;       // Earth = 1.0
        public bool HasWater;
        public bool IsHabitable;
        public bool LifeSeeded;
    }

    public class PlanetSimulator : ITemporalSimulator
    {
        public void Simulate(SimEntity entity, int years)
        {
            if (entity.Type != SimType.Planet)
                return;

            PlanetTraits traits;
            object raw;
            if (!entity.Traits.TryGetValue("traits", out raw) || !(raw is PlanetTraits))
            {
                traits = GenerateInitialTraits(entity.Seed);
                entity.Traits["traits"] = traits;
            }
            else
            {
                traits = (PlanetTraits)raw;
            }

            var rng = new Random(entity.Seed);

            for (int y = 0; y < years; y++)
            {
                traits.Age++;

                if (!traits.LifeSeeded && traits.IsHabitable && rng.NextDouble() < 0.01)
                {
                    traits.LifeSeeded = true;

                    entity.Timeline.Events.Add(new HistoricalEvent(
                        TemporalResolutionEngine.CurrentYear + y,
                        "Life Begins",
                        "Simple life has emerged on this planet."
                    ));
                }
            }
        }

        private PlanetTraits GenerateInitialTraits(int seed)
        {
            var rng = new Random(seed);

            var traits = new PlanetTraits();
            traits.Temperature = (float)(rng.NextDouble() * 150 - 50);  // -50°C to +100°C
            traits.Gravity = (float)(rng.NextDouble() * 2.0);           // 0–2g
            traits.HasWater = rng.NextDouble() < 0.6;
            traits.IsHabitable = traits.HasWater &&
                                 traits.Temperature > -10 &&
                                 traits.Temperature < 50 &&
                                 traits.Gravity > 0.5f &&
                                 traits.Gravity < 1.5f;

            return traits;
        }
    }
}
