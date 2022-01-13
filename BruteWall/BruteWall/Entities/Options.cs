using BruteWall.Interfaces;

namespace BruteWall.Entities
{
    public class Options : IOptions
    {
        public IStorage Storage { get; set; }
        public ushort StatusCode { get; set; }
        public ushort MaxRetries { get; set; }
        public BruteError Error { get; set; }
    }
}
