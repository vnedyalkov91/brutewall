using BruteWall.Entities;

namespace BruteWall.Interfaces
{
    public interface IStorage
    {
        public void AddEntity(BlockedAddress entity);

        public BlockedAddress GetEntity(string ipAddress);

        public void UpdateEntity(BlockedAddress entity);

        public void DeleteEntity(string ipAddress);
    }
}
