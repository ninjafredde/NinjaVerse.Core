using System;
using System.Collections.Generic;

namespace NinjaVerse.Core
{
    public class ObservedPath
    {
        public int GalaxyId;
        public int StarSystemId;
        public int PlanetId;
        public int CivilizationId;

        public override string ToString()
        {
            return $"Galaxy:{GalaxyId} → Star:{StarSystemId} → Planet:{PlanetId} → Civ:{CivilizationId}";
        }
    }

    public class ObserverContext
    {
        public int CurrentYear { get; private set; }
        public ObservedPath CurrentPath { get; private set; }

        private Dictionary<string, SimEntity> _observedEntities = new Dictionary<string, SimEntity>();

        public ObserverContext()
        {
            CurrentYear = 0;
            CurrentPath = new ObservedPath();
        }

        public void ObservePath(ObservedPath path, int year)
        {
            CurrentPath = path;
            CurrentYear = year;
        }

        public string GetPathKey(ObservedPath path, int year)
        {
            return $"{path.GalaxyId}-{path.StarSystemId}-{path.PlanetId}-{path.CivilizationId}-Y{year}";
        }

        public void StoreObservedEntity(ObservedPath path, int year, SimEntity entity)
        {
            var key = GetPathKey(path, year);
            _observedEntities[key] = entity;
        }

        public SimEntity TryGetObservedEntity(ObservedPath path, int year)
        {
            var key = GetPathKey(path, year);
            if (_observedEntities.ContainsKey(key))
                return _observedEntities[key];

            return null;
        }

        public void StepForward(int years, Action<ObservedPath, int> onStep = null)
        {
            for (int i = 1; i <= years; i++)
            {
                CurrentYear++;
                onStep?.Invoke(CurrentPath, CurrentYear);
            }
        }

        public void Reset()
        {
            _observedEntities.Clear();
            CurrentYear = 0;
            CurrentPath = new ObservedPath();
        }
    }
}
