using System;

using NinjaVerse.Core;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Models;

class Program
{
    public static int DeriveSeed(int baseSeed, int modifier)
    {
        // Example bit manipulation to derive a new seed
        return (baseSeed ^ (modifier << 5)) & 0x7FFFFFFF; // XOR and mask to ensure positive seed
    }

    static void Main(string[] args)
    {
        CosmicFate fate = CosmicFate.BigFreeze; // Example cosmic fate
        float universeAge = 0.2f; // Normalized universe age (0 to 1)

        // Create a Random instance
        Random random = new Random();

        // Calculate the number of galaxies
        int galaxyCount = UniverseCalculator.CalculateGalaxies(universeAge, fate);

        // Counters for logging
        int starSystemCount = 0;
        int planetCount = 0;
        int planetsWithLife = 0;
        int planetsWithCivilizations = 0;
        int deadGalaxies = 0;
        int deadStars = 0;

        // Species and Civilization Counters
        int speciesCreated = 0;
        int speciesExtinct = 0;
        int speciesConscious = 0;
        int civilizationsCreated = 0;
        int civilizationsDestroyed = 0;

        // Simulate galaxies
        for (int galaxyIndex = 1; galaxyIndex <= galaxyCount; galaxyIndex++)
        {
            var galaxy = new SimEntity(SimType.Galaxy, galaxyIndex);
            var galaxySimulator = new GalaxySimulator();
            galaxySimulator.Simulate(galaxy, universeAge, fate);

            if (galaxy.Traits["status"].ToString() == "dead")
            {
                deadGalaxies++;
            }

            int starSystemsInGalaxy = UniverseCalculator.CalculateStarSystemsPerGalaxy(universeAge, fate);
            starSystemCount += starSystemsInGalaxy;

            // Simulate star systems
            for (int starSystemIndex = 1; starSystemIndex <= starSystemsInGalaxy; starSystemIndex++)
            {
                var starSystem = new SimEntity(SimType.StarSystem, starSystemIndex);
                var starSystemSimulator = new StarSystemSimulator();
                starSystemSimulator.Simulate(starSystem, universeAge, fate);

                if (starSystem.Traits["status"].ToString() == "dark")
                {
                    deadStars++;
                }

                int planetsInStarSystem = UniverseCalculator.CalculatePlanetsPerStarSystem(universeAge, fate);
                planetCount += planetsInStarSystem;

                // Simulate planets
                for (int planetIndex = 1; planetIndex <= planetsInStarSystem; planetIndex++)
                {
                    float lifeProbability = UniverseCalculator.CalculateLifeProbability(universeAge, fate);
                    if (random.NextDouble() < lifeProbability)
                    {
                        planetsWithLife++;

                        // Simulate species
                        var speciesSimulator = new SpeciesSimulator();
                        var species = new SimEntity(SimType.Creature, planetIndex);
                        speciesSimulator.Simulate(species, universeAge); // Pass normalized age
                        speciesCreated++;

                        if (species.Traits.ContainsKey("status") && species.Traits["status"].ToString() == "extinct")
                        {
                            speciesExtinct++;
                        }
                        if (species.Traits.ContainsKey("conscious") && (bool)species.Traits["conscious"])
                        {
                            speciesConscious++;
                        }

                        // Simulate civilizations
                        float civilizationProbability = UniverseCalculator.CalculateCivilizationProbability(universeAge, fate);
                        if (random.NextDouble() < civilizationProbability)
                        {
                            planetsWithCivilizations++;
                            var civilizationSimulator = new CivilizationSimulator();
                            var civilization = new SimEntity(SimType.Civilization, planetIndex);
                            civilizationSimulator.Simulate(civilization, universeAge); // Pass normalized age
                            civilizationsCreated++;

                            if (civilization.Traits.ContainsKey("status") && civilization.Traits["status"].ToString() == "destroyed")
                            {
                                civilizationsDestroyed++;
                            }
                        }
                    }
                }
            }
        }

        // Log the results
        Console.WriteLine($"\nCosmic Fate: {fate}");
        Console.WriteLine($"At age {universeAge * 100:F1}% of the universe's lifespan:");
        Console.WriteLine($"  {galaxyCount} galaxies ({deadGalaxies} dead)");
        Console.WriteLine($"  {starSystemCount} star systems ({deadStars} dark)");
        Console.WriteLine($"  {planetCount} planets");
        Console.WriteLine($"  {planetsWithLife} planets with life");
        Console.WriteLine($"  {planetsWithCivilizations} planets with civilizations");
        Console.WriteLine($"  {speciesCreated} species created ({speciesExtinct} extinct, {speciesConscious} reached consciousness)");
        Console.WriteLine($"  {civilizationsCreated} civilizations created ({civilizationsDestroyed} destroyed)");
    }

    static void PrintTimeline(string title, TimelineData timeline)
    {
        Console.WriteLine($"\n--- {title} ---");
        foreach (var e in timeline.Events)
        {
            Console.WriteLine($"[{e.Year}] {e.Type} → {e.Description}");
        }
    }

    public static void ObservePlanet(SimEntity planet)
    {
        if (planet.Type != SimType.Planet) return;

        Console.WriteLine($"  Life Probability: {planet.LifeProbability:F2}");

        if (planet.LifeProbability > 0.8f)
        {
            // Materialize species traits
            var speciesTraits = new SpeciesTraits
            {
                Form = SpeciesForm.CarbonBased,
                Lifespan = 80,
                Cognition = 0.5f,
                Adaptability = 0.7f,
                Sociality = 0.6f,
                Resilience = 0.8f
            };

            planet.Traits["species"] = speciesTraits;
            Console.WriteLine("  Life observed on the planet!");
        }
        else
        {
            Console.WriteLine("  No life detected on the planet.");
        }
    }

    public static void ObserveCivilization(SimEntity civilization)
    {
        if (civilization.Type != SimType.Civilization) return;

        if (civilization.Traits.TryGetValue("traits", out var rawTraits) && rawTraits is CivilizationTraits traits)
        {
            Console.WriteLine("\n🌌 Civilization Details:");
            Console.WriteLine($"  Stage: {traits.Stage}");
            Console.WriteLine($"  Population: {traits.Population}");
            Console.WriteLine($"  Government: {traits.Government}");
            Console.WriteLine($"  Technology Level: {traits.TechnologyLevel:F2}");
            Console.WriteLine($"  Stability: {traits.Stability:F2}");
            Console.WriteLine($"  Knowledge: {traits.Knowledge:F2}");
        }
        else
        {
            Console.WriteLine("No civilization traits found.");
        }
    }
}
