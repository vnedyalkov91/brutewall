using BruteWall.Entities;
using BruteWall.Interfaces;
using BruteWall.Services;
using NUnit.Framework;

namespace BruteWall.Tests
{
    public class Initialize
    {

        [Test]
        public void Withoud_Parameters()
        {
            IOptions options = new Options();
            IBackendService service = new BackendService(options);

            Assert.Pass();
        }

        [Test]
        public void Partial_Parameters()
        {
            IOptions options = new Options() { StatusCode = 200, MaxRetries = 5 };
            IBackendService service = new BackendService(options);

            Assert.Pass();
        }

        [Test]
        public void Full_Parameters()
        {
            IOptions options = new Options() { 
                StatusCode = 200, 
                MaxRetries = 5, 
                Error = new BruteError() { Error = "Unauthorized", Message = "No go my friend", StatusCode = 550 } 
            };
            IBackendService service = new BackendService(options);

            Assert.Pass();
        }
    }
}