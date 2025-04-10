using System;
using NinjaVerse.Core.Enums;
public static class UniverseCalculator
{
    public static int CalculateGalaxies(float normalizedAge, CosmicFate fate)
    {
        double maxGalaxies = 1000;
        double growthRate = GetGrowthRate(fate);

        // Adjust growth based on cosmic fate
        return (int)(maxGalaxies / (1 + Math.Exp(-growthRate * (normalizedAge - 0.5))));
    }

    public static int CalculateStarSystemsPerGalaxy(float normalizedAge, CosmicFate fate)
    {
        double maxStarSystems = 200;
        double growthRate = GetGrowthRate(fate);

        // Adjust growth based on cosmic fate
        return (int)(maxStarSystems * Math.Pow(normalizedAge, growthRate));
    }

    public static int CalculatePlanetsPerStarSystem(float normalizedAge, CosmicFate fate)
    {
        double maxPlanets = 15;
        double growthRate = GetGrowthRate(fate);

        // Adjust growth based on cosmic fate
        return (int)(maxPlanets * Math.Pow(normalizedAge, growthRate));
    }

    public static float CalculateLifeProbability(float normalizedAge, CosmicFate fate)
    {
        double baseProbability = 0.1;
        double growthRate = GetGrowthRate(fate);

        // Adjust probability based on cosmic fate
        return (float)Math.Min(1.0, baseProbability + normalizedAge * growthRate);
    }

    public static float CalculateCivilizationProbability(float normalizedAge, CosmicFate fate)
    {
        double baseProbability = 0.05;
        double growthRate = GetGrowthRate(fate);

        // Adjust probability based on cosmic fate
        return (float)Math.Min(0.5, baseProbability + normalizedAge * growthRate);
    }

    public static float CalculateGalaxyDeathProbability(float normalizedAge, CosmicFate fate)
    {
        double baseProbability = 0.01; // Base chance of galaxy death
        double growthRate = GetGrowthRate(fate);

        // Probability increases as the universe approaches its end
        return (float)Math.Min(1.0, baseProbability + normalizedAge * growthRate * 0.5);
    }

    public static float CalculateStarDeathProbability(float normalizedAge, CosmicFate fate)
    {
        double baseProbability = 0.05; // Base chance of star death
        double growthRate = GetGrowthRate(fate);

        // Probability increases as the universe approaches its end
        return (float)Math.Min(1.0, baseProbability + normalizedAge * growthRate * 0.8);
    }

    private static double GetGrowthRate(CosmicFate fate)
    {
        // Define growth rates based on cosmic fate
        switch (fate)
        {
            case CosmicFate.BigFreeze:
                return 0.5;  // Slower growth
            case CosmicFate.BigCrunch:
                return 2.0;  // Faster growth
            case CosmicFate.BigRip:
                return 1.5;  // Peaking growth
            case CosmicFate.HeatDeath:
                return 0.3;  // Very slow growth
            default:
                return 1.0;  // Default growth
        }
    }
}
