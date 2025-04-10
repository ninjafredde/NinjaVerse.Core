using NinjaVerse.Core;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Models;
using System;

public class GalaxyTraits
{
    public int StarCount = 0;
    public int Age = 0;
    public int BlackHoleEvents = 0;
    public int SystemSeedsGenerated = 0;
}

public class GalaxySimulator
{
    public void Simulate(SimEntity galaxy, float normalizedAge, CosmicFate fate)
    {
        float deathProbability = UniverseCalculator.CalculateGalaxyDeathProbability(normalizedAge, fate);

        // Determine if the galaxy dies
        if (new Random().NextDouble() < deathProbability)
        {
            galaxy.Traits["status"] = "dead";
        }
        else
        {
            galaxy.Traits["status"] = "alive";
        }
    }
}
