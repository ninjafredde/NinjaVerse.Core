using System;
using NinjaVerse.Core;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Models;
public class StarSystemTraits
{
    public int Age = 0;
    public int StarCount = 1;
    public int PlanetCount = 0;
    public int HabitablePlanets = 0;
    public int PlanetSeedsGenerated = 0;
}

public class StarSystemSimulator
{
    public void Simulate(SimEntity starSystem, float normalizedAge, CosmicFate fate)
    {
        float deathProbability = UniverseCalculator.CalculateStarDeathProbability(normalizedAge, fate);

        // Determine if the star system dies
        if (new Random().NextDouble() < deathProbability)
        {
            starSystem.Traits["status"] = "dark";
        }
        else
        {
            starSystem.Traits["status"] = "active";
        }
    }
}
