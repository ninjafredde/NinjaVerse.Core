using System;
using NinjaVerse.Core.Enums;
namespace NinjaVerse.Core
{
    public class NPCFactory
    {
        public SimEntity CreateNPC(int seed)
        {
            return new SimEntity(SimType.NPC, seed);
        }
    }
}