using System;
using NinjaVerse.Core;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Timeline;

class Program
{
    static void Main(string[] args)
    {
        int rootSeed = 86753091;

        // Initialize simulators
        var universeSim = new UniverseSimulator();
        var speciesSim = new SpeciesSimulator();
        var civSim = new CivilizationSimulator();

        // Create Universe
        var universe = new SimEntity(SimType.Universe, rootSeed);
        universeSim.Simulate(universe, 20000);

        // Create Planet with potential for life
        var planet = new SimEntity(SimType.Planet, rootSeed + 1);
        speciesSim.Simulate(planet, 1000);

        // Check if sentient species emerged
        if (planet.Traits.TryGetValue("species", out var speciesRaw) && speciesRaw is SpeciesTraits speciesTraits)
        {
            // Print species traits
            Console.WriteLine("\n🌍 Species Traits:");
            Console.WriteLine($"  Form: {speciesTraits.Form}");
            Console.WriteLine($"  Lifespan: {speciesTraits.Lifespan} years");
            Console.WriteLine($"  Cognition: {speciesTraits.Cognition:F2}");
            Console.WriteLine($"  Adaptability: {speciesTraits.Adaptability:F2}");
            Console.WriteLine($"  Sociality: {speciesTraits.Sociality:F2}");
            Console.WriteLine($"  Resilience: {speciesTraits.Resilience:F2}");

            if (speciesTraits.Cognition >= 1.0f)
            {
                Console.WriteLine("\n🧬 Sentient species detected! Creating civilization...\n");

                var civ = new SimEntity(SimType.Civilization, rootSeed + 2);
                civSim.Simulate(civ, 500);

                PrintTimeline("Civilization Timeline", civ.Timeline);
            }
            else
            {
                Console.WriteLine("\n💀 Species did not reach sentience.\n");
            }
        }

        PrintTimeline("Universe Timeline", universe.Timeline);
        PrintTimeline("Planet Timeline", planet.Timeline);

        Console.WriteLine("\nSimulation complete.");
    }

    static void PrintTimeline(string title, TimelineData timeline)
    {
        Console.WriteLine($"\n--- {title} ---");
        foreach (var e in timeline.Events)
        {
            Console.WriteLine($"[{e.Year}] {e.Type} → {e.Description}");
        }
    }
}
