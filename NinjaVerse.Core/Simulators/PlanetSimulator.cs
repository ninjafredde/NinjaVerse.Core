using System;
using NinjaVerse.Core;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Models;

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
    public override void Simulate(SimEntity planet, float normalizedAge)
    {
        if (planet.Type != SimType.Planet) return;

        Random rng = new Random(planet.Seed);

        // Simulate planetary traits
        var traits = GetOrCreatePlanetTraits(planet);

        // Simulate planetary events
        string planetaryEvent = DeterminePlanetaryEvent(rng);
        ApplyPlanetaryEvent(planet, traits, planetaryEvent, rng);

        // Update habitability based on traits
        traits.IsHabitable = traits.HasWater && traits.Temperature > -50 && traits.Temperature < 50;

        // Log habitability changes
        if (traits.IsHabitable && !planet.Traits.ContainsKey("habitable"))
        {
            planet.Traits["habitable"] = true;
            planet.Timeline.Events.Add(new HistoricalEvent(
                TemporalResolutionEngine.CurrentYear,
                "Planet Habitable",
                "The planet has become habitable."
            ));
        }
    }

    private PlanetTraits GetOrCreatePlanetTraits(SimEntity planet)
    {
        if (!planet.Traits.TryGetValue("planetTraits", out var rawTraits) || !(rawTraits is PlanetTraits traits))
        {
            traits = new PlanetTraits
            {
                Temperature = new Random(planet.Seed).Next(-100, 100),
                Gravity = 1.0f,
                HasWater = new Random(planet.Seed).NextDouble() > 0.5
            };
            planet.Traits["planetTraits"] = traits;
        }
        return traits;
    }

    private string DeterminePlanetaryEvent(Random rng)
    {
        // Randomly determine a planetary event
        double roll = rng.NextDouble();
        if (roll < 0.1) return "Asteroid Impact"; // 10% chance
        if (roll < 0.2) return "Black Hole Proximity"; // 10% chance
        if (roll < 0.5) return "Environmental Stability"; // 30% chance
        return "No Event"; // 50% chance
    }

    private void ApplyPlanetaryEvent(SimEntity planet, PlanetTraits traits, string planetaryEvent, Random rng)
    {
        switch (planetaryEvent)
        {
            case "Asteroid Impact":
                traits.Temperature -= 10; // Decrease temperature
                planet.Timeline.Events.Add(new HistoricalEvent(
                    TemporalResolutionEngine.CurrentYear,
                    "Asteroid Impact",
                    "A catastrophic asteroid hit the planet."
                ));
                break;

            case "Black Hole Proximity":
                traits.Gravity += 0.1f; // Increase gravity
                planet.Timeline.Events.Add(new HistoricalEvent(
                    TemporalResolutionEngine.CurrentYear,
                    "Black Hole Proximity",
                    "The planet is near a black hole."
                ));
                break;

            case "Environmental Stability":
                traits.Temperature += 5; // Increase temperature
                planet.Timeline.Events.Add(new HistoricalEvent(
                    TemporalResolutionEngine.CurrentYear,
                    "Environmental Stability",
                    "The planet's environment is stable."
                ));
                break;

            case "No Event":
                // No changes
                break;
        }
    }
}
