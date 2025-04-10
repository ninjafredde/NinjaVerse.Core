using System;
using System.Collections.Generic;
using NinjaVerse.Core;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Models;


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
    public override void Simulate(SimEntity planet, float normalizedAge)
    {
        if (planet.Type != SimType.Planet) return;

        Random rng = new Random(planet.Seed + TemporalResolutionEngine.CurrentYear); // Ensure randomness

        // Check for species creation
        if (planet.Traits.TryGetValue("habitable", out var isHabitable) && (bool)isHabitable)
        {
            if (!planet.Traits.ContainsKey("species"))
            {
                var species = GenerateSpeciesTraits(rng.Next());
                planet.Traits["species"] = species;
                planet.Timeline.Events.Add(new HistoricalEvent(
                    TemporalResolutionEngine.CurrentYear,
                    "Species Created",
                    $"A new species has emerged with Cognition: {species.Cognition:F2}."
                ));
                Console.WriteLine($"Species created with Cognition: {species.Cognition:F2}");
            }
        }

        // Check for species extinction
        if (planet.Traits.ContainsKey("species"))
        {
            double extinctionRoll = rng.NextDouble();
            Console.WriteLine($"Extinction roll: {extinctionRoll:F2}");
            if (extinctionRoll < 0.1) // Increased extinction probability to 10%
            {
                planet.Traits.Remove("species");
                planet.Timeline.Events.Add(new HistoricalEvent(
                    TemporalResolutionEngine.CurrentYear,
                    "Species Extinct",
                    "A species has gone extinct."
                ));
                Console.WriteLine("A species has gone extinct.");
            }
        }

        // Check for species consciousness
        if (planet.Traits.ContainsKey("species"))
        {
            var species = (SpeciesTraits)planet.Traits["species"];

            // Gradually increase Cognition over time
            species.Cognition += (float)(0.01 * normalizedAge); // Increase Cognition based on universe age
            species.Cognition = Math.Min(1.0f, species.Cognition); // Cap at 1.0
            Console.WriteLine($"Updated Cognition: {species.Cognition:F2}");

            if (species.Cognition > 0.9f && !planet.Traits.ContainsKey("conscious"))
            {
                planet.Traits["conscious"] = true;
                planet.Timeline.Events.Add(new HistoricalEvent(
                    TemporalResolutionEngine.CurrentYear,
                    "Consciousness Achieved",
                    "A species has reached consciousness."
                ));
                Console.WriteLine("A species has reached consciousness.");
            }
        }
    }

    public static SpeciesTraits GenerateSpeciesTraits(int seed)
    {
        var rng = new Random(seed);

        return new SpeciesTraits
        {
            Form = (SpeciesForm)(rng.Next(Enum.GetValues(typeof(SpeciesForm)).Length)),
            Lifespan = rng.Next(5, 150),
            Cognition = (float)(rng.NextDouble() * 0.5), // Start with lower Cognition
            Adaptability = (float)(rng.NextDouble()),
            Sociality = (float)(rng.NextDouble()),
            Resilience = (float)(rng.NextDouble() * 0.9 + 0.1f)
        };
    }
}
