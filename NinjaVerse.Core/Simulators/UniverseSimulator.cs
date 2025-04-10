using NinjaVerse.Core;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Models;
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





public class UniverseSimulator
{
    public void Simulate(SimEntity universe, int universeAge)
    {
        // Example: Number of galaxies increases with age
        int maxGalaxies = Math.Min(1000, universeAge / 1000); // Cap at 1000 galaxies
        universe.Traits["galaxyCount"] = maxGalaxies;
    }
}
