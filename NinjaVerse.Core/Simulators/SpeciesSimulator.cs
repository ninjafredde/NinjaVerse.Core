using System;
using System.Collections.Generic;

namespace NinjaVerse.Core
{
    public enum SpeciesForm
    {
        CarbonBased, SiliconBased, EnergyBased, PlasmaEntity,
        Crystalline, Insectoid, Amphibious, Synthetic, Unknown
    }

    public class SpeciesTraits
    {
        public SpeciesForm Form;
        public float Lifespan;
        public float Cognition;        // Sentience threshold ≈ 1.0
        public float Adaptability;
        public float Sociality;
        public float Resilience;
        public float EvolutionScore;
    }

    public class SpeciesSimulator : ITemporalSimulator
    {
        public void Simulate(SimEntity entity, int years)
        {
            if (entity.Type != SimType.Planet) return;

            SpeciesTraits species;
            if (!entity.Traits.TryGetValue("species", out var raw) || !(raw is SpeciesTraits))
            {
                species = GenerateBaseSpecies(entity.Seed);
                entity.Traits["species"] = species;
            }
            else species = (SpeciesTraits)raw;

            Random rng = new Random(entity.Seed);

            for (int y = 0; y < years; y++)
            {
                species.EvolutionScore += (float)(rng.NextDouble() * 0.01);
                if (rng.NextDouble() < 0.0001)
                    species.Cognition += (float)(rng.NextDouble() * 0.05f);

                if (rng.NextDouble() > species.Resilience)
                {
                    entity.Timeline.Events.Add(new HistoricalEvent(
                        TemporalResolutionEngine.CurrentYear,
                        "Extinction Event",
                        "A mass extinction event occurred — survival uncertain."
                    ));

                    if (rng.NextDouble() > 0.3f)
                    {
                        entity.Timeline.Events.Add(new HistoricalEvent(
                            TemporalResolutionEngine.CurrentYear,
                            "Extinction",
                            "The species has perished before reaching sentience."
                        ));
                        return;
                    }
                }

                if (species.Cognition >= 1.0f)
                {
                    entity.Timeline.Events.Add(new HistoricalEvent(
                        TemporalResolutionEngine.CurrentYear,
                        "Sentience",
                        "A new intelligent species has emerged."
                    ));
                    break;
                }
            }
        }

        private SpeciesTraits GenerateBaseSpecies(int seed)
        {
            var rng = new Random(seed);
            return new SpeciesTraits
            {
                Form = (SpeciesForm)(rng.Next(Enum.GetValues(typeof(SpeciesForm)).Length)),
                Lifespan = rng.Next(5, 150),
                Cognition = (float)(rng.NextDouble() * 0.5),
                Adaptability = (float)(rng.NextDouble()),
                Sociality = (float)(rng.NextDouble()),
                Resilience = (float)(rng.NextDouble() * 0.9 + 0.1f)
            };
        }
    }
}
