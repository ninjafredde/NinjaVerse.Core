using System;
using System.Collections.Generic;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Timeline;

namespace NinjaVerse.Core
{
    public enum CivilizationEpoch
    {
        Primitive,
        Agrarian,
        Industrial,
        Information,
        Spacefaring,
        Singularity,
        PostSingularity
    }

    public class CivilizationTraits
    {
        public CivilizationEpoch Epoch = CivilizationEpoch.Primitive;
        public int Age = 0;
        public int Population = 1000;
        public int TechLevel = 0;
        public float Stability = 1f;
        public float Knowledge = 0f;
    }

    public class CivilizationSimulator : ITemporalSimulator
    {
        public void Simulate(SimEntity entity, int years)
        {
            if (entity.Type != SimType.Civilization) return;

            CivilizationTraits traits;
            object raw;
            if (!entity.Traits.TryGetValue("traits", out raw) || !(raw is CivilizationTraits))
            {
                traits = new CivilizationTraits();
                entity.Traits["traits"] = traits;
            }
            else
            {
                traits = (CivilizationTraits)raw;
            }

            var rng = new Random(entity.Seed);

            for (int y = 0; y < years; y++)
            {
                traits.Age++;
                traits.Population += 10;
                traits.TechLevel += 1;
                traits.Knowledge += 0.5f;

                TryAdvanceEpoch(traits, entity);

                // Trigger NPC generation after a threshold
                if (traits.Age >= 50 && traits.Population >= 10000 && !entity.Traits.ContainsKey("npc_initialized"))
                {
                    var npcFactory = new NPCFactory();
                    var npcList = new List<SimEntity>();

                    for (int i = 0; i < 10; i++)
                    {
                        var npc = npcFactory.CreateNPC(entity.Seed + i);
                        var npcSim = new NPCSimulator();
                        npcSim.Simulate(npc, 10);
                        npcList.Add(npc);
                    }

                    entity.Traits["npcs"] = npcList;
                    entity.Traits["npc_initialized"] = true;

                    entity.Timeline.Events.Add(new HistoricalEvent(
                        TemporalResolutionEngine.CurrentYear + y,
                        "Population Emergence",
                        "NPC individuals have formed within the civilization."
                    ));
                }
            }

            entity.Timeline.Events.Add(new HistoricalEvent(
                TemporalResolutionEngine.CurrentYear + years,
                "Simulation",
                $"Civ aged {years}y, Epoch: {traits.Epoch}, Pop: {traits.Population}, Tech: {traits.TechLevel}"
            ));
        }

        void TryAdvanceEpoch(CivilizationTraits traits, SimEntity entity)
        {
            var year = TemporalResolutionEngine.CurrentYear;
            switch (traits.Epoch)
            {
                case CivilizationEpoch.Primitive:
                    if (traits.TechLevel >= 10)
                    {
                        traits.Epoch = CivilizationEpoch.Agrarian;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Agrarian society emerges."));
                    }
                    break;
                case CivilizationEpoch.Agrarian:
                    if (traits.TechLevel >= 30)
                    {
                        traits.Epoch = CivilizationEpoch.Industrial;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Industrialization begins."));
                    }
                    break;
                case CivilizationEpoch.Industrial:
                    if (traits.TechLevel >= 50)
                    {
                        traits.Epoch = CivilizationEpoch.Information;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Digital revolution takes hold."));
                    }
                    break;
                case CivilizationEpoch.Information:
                    if (traits.TechLevel >= 75)
                    {
                        traits.Epoch = CivilizationEpoch.Spacefaring;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Civilization ventures into space."));
                    }
                    break;
                case CivilizationEpoch.Spacefaring:
                    if (traits.TechLevel >= 100)
                    {
                        traits.Epoch = CivilizationEpoch.Singularity;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Singularity reached. Tech merges with mind."));
                    }
                    break;
                case CivilizationEpoch.Singularity:
                    if (traits.TechLevel >= 120)
                    {
                        traits.Epoch = CivilizationEpoch.PostSingularity;
                        entity.Timeline.Events.Add(new HistoricalEvent(year, "Epoch Shift", "Civ transcends biology. Post-human era begins."));
                    }
                    break;
            }
        }
    }
}
