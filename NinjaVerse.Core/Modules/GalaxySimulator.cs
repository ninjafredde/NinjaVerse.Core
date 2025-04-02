using NinjaVerse.Core;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Timeline;
using System;

public class GalaxyTraits
{
    public int StarCount = 0;
    public int Age = 0;
    public int BlackHoleEvents = 0;
    public int SystemSeedsGenerated = 0;
}

public class GalaxySimulator : ITemporalSimulator
{
    public void Simulate(SimEntity entity, int years)
    {
        if (entity.Type != SimType.Galaxy) return;

        GalaxyTraits traits;
        if (!entity.Traits.TryGetValue("traits", out var raw) || !(raw is GalaxyTraits))
        {
            traits = new GalaxyTraits();
            entity.Traits["traits"] = traits;
        }
        else traits = (GalaxyTraits)raw;

        var rng = new Random(entity.Seed);

        for (int y = 0; y < years; y++)
        {
            traits.Age++;

            // Random chance to form new stars in batches
            if (rng.NextDouble() < 0.01)
            {
                int newStars = rng.Next(1000, 100000);
                traits.StarCount += newStars;
                entity.Timeline.Events.Add(new HistoricalEvent(
                    TemporalResolutionEngine.CurrentYear + y,
                    "Star Formation",
                    $"{newStars} stars formed. Total: {traits.StarCount}"
                ));
            }

            // Rare cosmic event
            if (rng.NextDouble() < 0.0005)
            {
                traits.BlackHoleEvents++;
                entity.Timeline.Events.Add(new HistoricalEvent(
                    TemporalResolutionEngine.CurrentYear + y,
                    "Black Hole",
                    "A supermassive black hole destabilized part of the galaxy."
                ));
            }

            // Seeding star systems
            if (traits.StarCount > 0 && rng.NextDouble() < 0.02)
            {
                traits.SystemSeedsGenerated++;
                entity.Timeline.Events.Add(new HistoricalEvent(
                    TemporalResolutionEngine.CurrentYear + y,
                    "System Seed",
                    "New star system ready for simulation."
                ));
            }
        }
    }
}