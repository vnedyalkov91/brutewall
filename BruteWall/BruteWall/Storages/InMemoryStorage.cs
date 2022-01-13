using BruteWall.Entities;
using BruteWall.Interfaces;
using System.Collections.Generic;

namespace BruteWall.Storages
{
    internal class InMemoryStorage : IStorage
    {
        private List<BlockedAddress> _store;

        public InMemoryStorage()
        {
            this._store = new List<BlockedAddress>();
        }

        public void AddEntity(BlockedAddress entity)
        {
            this._store.Add(entity);
        }

        public BlockedAddress GetEntity(string ipAddress)
        {
            BlockedAddress entity = this._store.Find(e => e.IpAddress == ipAddress);
            return entity;
        }

        public void UpdateEntity(BlockedAddress entity)
        {
            int index = this._store.FindIndex(e => e.IpAddress == entity.IpAddress);
            this._store[index] = entity;
        }

        public void DeleteEntity(string ipAddress)
        {
            this._store.Remove(this._store.Find(e => e.IpAddress == ipAddress));
        }
    }
}
