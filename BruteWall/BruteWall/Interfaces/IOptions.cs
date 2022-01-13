using BruteWall.Entities;

namespace BruteWall.Interfaces
{
    public interface IOptions
    {
        public IStorage Storage { get; set; }

        public ushort StatusCode { get; set; }

        public ushort MaxRetries { get; set; }

        public BruteError Error { get; set; }
    }
}
