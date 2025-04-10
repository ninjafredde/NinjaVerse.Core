using NinjaVerse.Core.Enums;
using NinjaVerse.Core.Models;
using System;

public abstract class ITemporalSimulator
{
    public abstract void Simulate(SimEntity entity, float years);
}