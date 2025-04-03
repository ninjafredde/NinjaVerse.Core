using System;
using System.Collections.Generic;

namespace NinjaVerse.Core
{
    public class SimulationStore
    {
        private readonly Dictionary<string, SimEntity> _entityCache = new Dictionary<string, SimEntity>();
        private readonly int _maxEntities;

        public SimulationStore(int maxEntities = 1000)
        {
            _maxEntities = maxEntities;
        }

        public void Store(string key, SimEntity entity)
        {
            if (_entityCache.ContainsKey(key))
                return;

            if (_entityCache.Count >= _maxEntities)
            {
                // Basic eviction: remove first item (FIFO)
                var firstKey = new List<string>(_entityCache.Keys)[0];
                _entityCache.Remove(firstKey);
            }

            _entityCache[key] = entity;
        }

        public SimEntity Retrieve(string key)
        {
            if (_entityCache.TryGetValue(key, out var entity))
                return entity;

            return null;
        }

        public bool Contains(string key) => _entityCache.ContainsKey(key);

        public void Clear()
        {
            _entityCache.Clear();
        }

        public int Count => _entityCache.Count;
    }
}
