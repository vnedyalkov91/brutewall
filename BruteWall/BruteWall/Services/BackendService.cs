using BruteWall.Entities;
using BruteWall.Interfaces;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BruteWall.Tests")]
namespace BruteWall.Services
{
    internal class BackendService : IBackendService
    {
        private readonly ushort _maxRetries;
        private readonly IStorage _storage;

        public ushort _statusCode { get; }

        public BruteError _error { get; }

        public BackendService(IOptions options)
        {
            this._storage = options.Storage;
            this._statusCode = options.StatusCode;
            this._maxRetries = options.MaxRetries;
            this._error = options.Error;
        }

        public BlockedAddress RegisterSender(string remoteIpAddress)
        {
            return RegisterSender(remoteIpAddress, this._maxRetries);
        }

        public BlockedAddress RegisterSender(string remoteIpAddress, ushort maxReties)
        {
            BlockedAddress existingEntity = this._storage.GetEntity(remoteIpAddress);

            if (existingEntity != null)
            {
                if (existingEntity.Retries <= maxReties)
                {
                    existingEntity.Retries++;
                    existingEntity.LastTry = DateTime.Now;
                    this._storage.UpdateEntity(existingEntity);
                    return existingEntity;
                }
                else
                {
                    existingEntity.Retries++;
                    existingEntity.LastTry = DateTime.Now;
                    existingEntity.IsLocked = true;
                    existingEntity.DurationInSecunds = (ulong)this.GetFibbonaciNumber(
                        existingEntity.Retries - this._maxRetries) * 60;
                    this._storage.UpdateEntity(existingEntity);
                    return existingEntity;
                }
            }
            else
            {
                BlockedAddress newEntity = new BlockedAddress()
                {
                    IpAddress = remoteIpAddress,
                    LastTry = DateTime.Now,
                    Retries = 1,
                    DurationInSecunds = 0,
                    IsLocked = false,
                };

                this._storage.AddEntity(newEntity);

                return newEntity;
            }
        }

        public void EraseSender(string remoteIpAddress)
        {
            BlockedAddress existingEntity = this._storage.GetEntity(remoteIpAddress);

            if (existingEntity != null)
            {
                DateTime dueDate = existingEntity.LastTry.AddSeconds(existingEntity.DurationInSecunds);
                if ((dueDate < DateTime.Now) && existingEntity.IsLocked == true)
                {
                    this._storage.DeleteEntity(remoteIpAddress);
                }
            }
        }

        private int GetFibbonaciNumber(int n)
        {
            if (n <= 2)
                return 1;
            else
            {
                return GetFibbonaciNumber(n - 1) + GetFibbonaciNumber(n - 2);
            }
        }
    }
}
