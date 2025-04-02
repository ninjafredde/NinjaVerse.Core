using NinjaVerse.Core;
using System;

public class UniverseTraits
{
    public int Age;
    public float ExpansionRate;
    public float MatterDensity;
    public int GalaxyCount;
    public int StarBirths;
    public CosmicFate Fate;
}

public interface ITemporalSimulator
{
    void Simulate(SimEntity entity, int years);
}

public static class TemporalResolutionEngine
{
    public static int CurrentYear = 0;
}

public class UniverseSimulator : ITemporalSimulator
{
    public void Simulate(SimEntity entity, int years)
    {
        if (entity.Type != SimType.Universe) return;

        UniverseTraits traits;
        if (!entity.Traits.TryGetValue("traits", out var rawTraits) || !(rawTraits is UniverseTraits))
        {
            traits = new UniverseTraits
            {
                Age = 0,
                ExpansionRate = 1f,
                MatterDensity = 1f,
                GalaxyCount = 0,
                StarBirths = 0,
                Fate = CosmicFate.Unknown
            };
            entity.Traits["traits"] = traits;
            entity.Timeline.Events.Add(new HistoricalEvent(0, "Big Bang", "The universe has begun."));
        }
        else
        {
            traits = (UniverseTraits)rawTraits;
        }

        Random rng = new Random(entity.Seed + TemporalResolutionEngine.CurrentYear);
        for (int y = 0; y < years; y++)
        {
            traits.Age++;
            TemporalResolutionEngine.CurrentYear++;
            traits.ExpansionRate *= 1.0001f;

            if (traits.Age > 100 && rng.NextDouble() < 0.01)
            {
                traits.GalaxyCount++;
                entity.Timeline.Events.Add(new HistoricalEvent(TemporalResolutionEngine.CurrentYear, "Galaxy Formed", "A galaxy has condensed from a mass cluster."));
            }

            if (traits.GalaxyCount > 10 && rng.NextDouble() < 0.001)
            {
                entity.Timeline.Events.Add(new HistoricalEvent(TemporalResolutionEngine.CurrentYear, "Galactic Merger", "Two galaxies have collided and merged."));
            }

            if (traits.GalaxyCount > 0 && rng.NextDouble() < 0.02)
            {
                traits.StarBirths++;
                entity.Timeline.Events.Add(new HistoricalEvent(TemporalResolutionEngine.CurrentYear, "Star Ignited", "A new star begins to burn."));
            }

            if (traits.StarBirths > 100 && rng.NextDouble() < 0.001)
            {
                entity.Timeline.Events.Add(new HistoricalEvent(TemporalResolutionEngine.CurrentYear, "Supernova", "A massive star explodes in a brilliant supernova."));
            }

            if (traits.StarBirths > 200 && rng.NextDouble() < 0.0005)
            {
                entity.Timeline.Events.Add(new HistoricalEvent(TemporalResolutionEngine.CurrentYear, "Black Hole", "A black hole has formed from stellar collapse."));
            }
        }

        entity.Timeline.Events.Add(new HistoricalEvent(TemporalResolutionEngine.CurrentYear, "Simulated", $"Universe aged {years} years. Galaxies: {traits.GalaxyCount}, Stars: {traits.StarBirths}"));
    }
}