using System.Collections.Generic;
using UnityEngine;
using NinjaVerse.Core;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Timeline;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance;

    [Header("Simulation Settings")]
    public int RootSeed = 42;
    public int UniverseYears = 500;

    [Header("Simulation State")]
    public SimEntity UniverseEntity;
    public List<SimEntity> Galaxies = new List<SimEntity>();
    public List<SimEntity> StarSystems = new List<SimEntity>();
    public List<SimEntity> Planets = new List<SimEntity>();
    public List<SimEntity> Civilizations = new List<SimEntity>();
    public List<SimEntity> NPCs = new List<SimEntity>();

    // Simulators
    private UniverseSimulator universeSim;
    private GalaxySimulator galaxySim;
    private StarSystemSimulator starSystemSim;
    private PlanetSimulator planetSim;
    private SpeciesSimulator speciesSim;
    private CivilizationSimulator civSim;
    private NPCSimulator npcSim;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        universeSim = new UniverseSimulator();
        galaxySim = new GalaxySimulator();
        starSystemSim = new StarSystemSimulator();
        planetSim = new PlanetSimulator();
        speciesSim = new SpeciesSimulator();
        civSim = new CivilizationSimulator();
        npcSim = new NPCSimulator();
    }

    public void RunSimulation()
    {
        ClearState();

        // Step 1: Simulate Universe
        UniverseEntity = new SimEntity(SimType.Universe, RootSeed);
        universeSim.Simulate(UniverseEntity, UniverseYears);

        // Step 2: Create 1 galaxy for demo purposes
        var galaxy = new SimEntity(SimType.Galaxy, RootSeed + 1);
        galaxySim.Simulate(galaxy, 200);
        Galaxies.Add(galaxy);

        // Step 3: Create 1 star system
        var starSystem = new SimEntity(SimType.StarSystem, RootSeed + 2);
        starSystemSim.Simulate(starSystem, 100);
        StarSystems.Add(starSystem);

        // Step 4: Create 1 planet
        var planet = new SimEntity(SimType.Planet, RootSeed + 3);
        planetSim.Simulate(planet, 1000);
        Planets.Add(planet);

        // Step 5: Simulate species
        speciesSim.Simulate(planet, 1000);

        // Step 6: If species is sentient, create civilization
        if (planet.Traits.ContainsKey("species"))
        {
            var species = planet.Traits["species"] as SpeciesTraits;
            if (species != null && species.Cognition >= 1.0f)
            {
                var civ = new SimEntity(SimType.Civilization, RootSeed + 4);
                civ.Traits["origin_species"] = species;
                civSim.Simulate(civ, 500);
                Civilizations.Add(civ);

                // Step 7: Load NPCs if present
                if (civ.Traits.ContainsKey("npcs"))
                {
                    var npcList = civ.Traits["npcs"] as List<SimEntity>;
                    if (npcList != null)
                        NPCs.AddRange(npcList);
                }
            }
        }
        Debug.Log($"🌌 Galaxies: {Galaxies.Count}");
        Debug.Log($"🪐 Planets: {Planets.Count}");
        Debug.Log($"🧬 Species evolved: {(Planets.Count > 0 && Planets[0].Traits.ContainsKey("species"))}");
        Debug.Log($"🏛️ Civilizations: {Civilizations.Count}");
        Debug.Log("✅ Simulation complete.");
    }
    public string GetSimulationSummary()
    {
        int totalCivs = Civilizations.Count;
        int activeCivs = 0;

        foreach (var civ in Civilizations)
        {
            var traits = civ.Traits.ContainsKey("traits") ? civ.Traits["traits"] as CivilizationTraits : null;
            if (traits != null && traits.Stability > 0.3f) // arbitrary cutoff
            {
                activeCivs++;
            }
        }

        return $"Seed: {RootSeed}\\n" +
               $"Years Simulated: {UniverseYears}\\n" +
               $"Galaxies: {Galaxies.Count}\\n" +
               $"Civilizations: {totalCivs} ({activeCivs} active)";
    }
    private void ClearState()
    {
        Galaxies.Clear();
        StarSystems.Clear();
        Planets.Clear();
        Civilizations.Clear();
        NPCs.Clear();
    }
}
