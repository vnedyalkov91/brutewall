using BruteWall.Entities;
using BruteWall.Interfaces;
using BruteWall.Services;
using BruteWall.Storages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BruteWall
{
    public static class BruteWallService
    {
        private static string _Error = "Too many requests";
        private static string _Message = "You have exceeded your request limit, please try again after :secunds seconds.";
        private static ushort _MaxRetiries = 3;

        public static void AddBruteWallService(this IServiceCollection services)
        {
            services.AddSingleton<IBackendService, BackendService>(x =>
                ActivatorUtilities.CreateInstance<BackendService>(
                    x, 
                    new Options()
                    {
                        Storage = new InMemoryStorage(),
                        StatusCode = StatusCodes.Status401Unauthorized,
                        MaxRetries = _MaxRetiries,
                        Error = new BruteError()
                        {
                            StatusCode = StatusCodes.Status429TooManyRequests,
                            Error = _Error,
                            Message = _Message
                        }
                    }
                )
            );
        }

        public static void AddBruteWallService(this IServiceCollection services, IOptions options)
        {
            services.AddSingleton<IBackendService, BackendService>(x =>
                ActivatorUtilities.CreateInstance<BackendService>(
                    x,
                    new Options()
                    {
                        Storage = options.Storage != null ? options.Storage : new InMemoryStorage(),
                        StatusCode = options.StatusCode != 0 ? options.StatusCode : (ushort) StatusCodes.Status401Unauthorized,
                        MaxRetries = options.MaxRetries != 0 ? options.MaxRetries : _MaxRetiries,
                        Error = options.Error != null ? options.Error : new BruteError()
                        {
                            StatusCode = StatusCodes.Status429TooManyRequests,
                            Error = _Error,
                            Message = _Message
                        }
                    }
                )
            );
        }
    }
}
