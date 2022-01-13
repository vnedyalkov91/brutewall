using System;

namespace BruteWall.Entities
{
    public class BlockedAddress
    {
        public string IpAddress { get; set; }

        public DateTime LastTry { get; set; }

        public int Retries { get; set; }

        public ulong DurationInSecunds { get; set; }

        public bool IsLocked { get; set; }
    }
}
