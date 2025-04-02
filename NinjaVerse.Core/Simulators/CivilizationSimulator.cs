using System;
using NinjaVerse.Core;

namespace NinjaVerse.Core
{
    public enum CivilizationEpoch
    {
        Primitive,
        Agrarian,
        Industrial,
        Information,
        Spacefaring,
        Singularity,
        PostSingularity
    }

    public class CivilizationTraits
    {
        public CivilizationEpoch Epoch = CivilizationEpoch.Primitive;
        public int Age = 0;
        public int Population = 1000;
        public int TechLevel = 0;
        public float Stability = 1f;
        public float Knowledge = 0f;
    }

    public class CivilizationSimulator : ITemporalSimulator
    {
        public void Simulate(SimEntity entity, int years)
        {
            if (entity.Type != SimType.Civilization) return;

            CivilizationTraits traits;
            if (!entity.Traits.TryGetValue("traits", out var raw) || !(raw is CivilizationTraits))
            {
                traits = new CivilizationTraits();
                entity.Traits["traits"] = traits;
            }
            else traits = (CivilizationTraits)raw;

            for (int y = 0; y < years; y++)
            {
                traits.Age++;
                traits.Population += 10;
                traits.TechLevel += 1;
                traits.Knowledge += 0.5f;

                TryAdvanceEpoch(traits, entity);
            }

            entity.Timeline.Events.Add(new HistoricalEvent(
                TemporalResolutionEngine.CurrentYear + years,
                "Simulation",
                $"Civ aged {years}y, Epoch: {traits.Epoch}, Pop: {traits.Population}, Tech: {traits.TechLevel}"
            ));
        }

        void TryAdvanceEpoch(CivilizationTraits traits, SimEntity entity)
        {
            var year = TemporalResolutionEngine.CurrentYear;
            switch (traits.Epoch)
            {
                case CivilizationEpoch.Primitive:
                    if (traits.TechLevel >= 10)
                    {
                        traits.Epoch = CivilizationEpoch.Agrarian;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Agrarian society emerges."));
                    }
                    break;
                case CivilizationEpoch.Agrarian:
                    if (traits.TechLevel >= 30)
                    {
                        traits.Epoch = CivilizationEpoch.Industrial;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Industrialization begins."));
                    }
                    break;
                case CivilizationEpoch.Industrial:
                    if (traits.TechLevel >= 50)
                    {
                        traits.Epoch = CivilizationEpoch.Information;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Digital revolution takes hold."));
                    }
                    break;
                case CivilizationEpoch.Information:
                    if (traits.TechLevel >= 75)
                    {
                        traits.Epoch = CivilizationEpoch.Spacefaring;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Civilization ventures into space."));
                    }
                    break;
                case CivilizationEpoch.Spacefaring:
                    if (traits.TechLevel >= 100)
                    {
                        traits.Epoch = CivilizationEpoch.Singularity;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Singularity reached. Tech merges with mind."));
                    }
                    break;
                case CivilizationEpoch.Singularity:
                    if (traits.TechLevel >= 120)
                    {
                        traits.Epoch = CivilizationEpoch.PostSingularity;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Civ transcends biology. Post-human era begins."));
                    }
                    break;
            }
        }
    }
}
