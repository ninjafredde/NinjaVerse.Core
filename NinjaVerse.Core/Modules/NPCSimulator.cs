using System;
using System.Collections.Generic;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Timeline;
namespace NinjaVerse.Core
{
    public class NPCTraits
    {
        public string Name;
        public int Age;
        public string Role;
        public string Class;
        public float Happiness;
        public float Loyalty;
    }

    public class NPCSimulator : ITemporalSimulator
    {
        public void Simulate(SimEntity entity, int years)
        {
            if (entity.Type != SimType.NPC) return;

            NPCTraits traits;
            object raw;
            if (!entity.Traits.TryGetValue("traits", out raw) || !(raw is NPCTraits))
            {
                traits = GenerateBaseTraits(entity.Seed);
                entity.Traits["traits"] = traits;
            }
            else
            {
                traits = (NPCTraits)raw;
            }

            var rng = new Random(entity.Seed);

            for (int y = 0; y < years; y++)
            {
                traits.Age++;

                float moodShift = (float)(rng.NextDouble() * 0.2 - 0.1);
                traits.Happiness = Math.Max(0f, Math.Min(1f, traits.Happiness + moodShift));

                traits.Loyalty += (traits.Happiness - 0.5f) * 0.05f;
                traits.Loyalty = Math.Max(0f, Math.Min(1f, traits.Loyalty));

                if (traits.Happiness < 0.2f)
                {
                    entity.Timeline.Events.Add(new HistoricalEvent(
                        TemporalResolutionEngine.CurrentYear + y,
                        "Unrest",
                        traits.Name + " is experiencing personal unrest."
                    ));
                }
            }
        }

        private NPCTraits GenerateBaseTraits(int seed)
        {
            var rng = new Random(seed);
            string[] names = { "Tora", "Lemin", "Caden", "Arra", "Juno", "Velk" };
            string[] roles = { "Worker", "Scholar", "Guard", "Leader", "Healer" };
            string[] classes = { "Commoner", "Noble", "Spiritual", "Scientific" };

            return new NPCTraits
            {
                Name = names[rng.Next(names.Length)],
                Age = rng.Next(18, 60),
                Role = roles[rng.Next(roles.Length)],
                Class = classes[rng.Next(classes.Length)],
                Happiness = (float)rng.NextDouble(),
                Loyalty = (float)rng.NextDouble()
            };
        }
    }
}
