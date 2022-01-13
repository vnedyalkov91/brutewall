using BruteWall.Entities;

namespace BruteWall.Interfaces
{
    public interface IBackendService
    {
        public ushort _statusCode { get; }

        public BruteError _error { get; }

        public BlockedAddress RegisterSender(string remoteIpAddress);

        public BlockedAddress RegisterSender(string remoteIpAddress, ushort maxReties);

        public void EraseSender(string remoteIpAddress);
    }
}
