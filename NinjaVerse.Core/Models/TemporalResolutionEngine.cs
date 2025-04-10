using System;
using System.Collections.Generic;
using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Models;
using NinjaVerse.Core;

public static class TemporalResolutionEngine
{
    public static int CurrentYear { get; private set; } = 0;

    private static readonly Dictionary<SimType, int> UpdateRates = new Dictionary<SimType, int>
    {
        { SimType.Creature, 1 },       // Every tick
        { SimType.Planet, 10 },       // Every 10 seconds
        { SimType.Civilization, 100 }, // Example: Every 100 ticks
        { SimType.StarSystem, 86400 }, // Every day
        { SimType.Galaxy, 3153600 }, // Every year
        { SimType.Universe, 31536000 } // Every 10 years
    };

    private static readonly Dictionary<SimType, int> LastUpdateTimes = new Dictionary<SimType, int>();

    public static void Initialize()
    {
        foreach (var simType in UpdateRates.Keys)
        {
            LastUpdateTimes[simType] = 0;
        }
    }

    public static bool ShouldUpdate(SimType simType, int currentTime)
    {
        int updateRate = UpdateRates[simType];
        int lastUpdateTime = LastUpdateTimes[simType];

        if (currentTime - lastUpdateTime >= updateRate)
        {
            LastUpdateTimes[simType] = currentTime;
            return true;
        }

        return false;
    }

    public static void AdvanceTime(int years)
    {
        CurrentYear += years;
    }
}

