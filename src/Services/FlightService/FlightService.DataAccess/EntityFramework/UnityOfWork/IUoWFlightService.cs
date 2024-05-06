using FlightService.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Core.Domain.Repositories;

namespace FlightService.DataAccess.EntityFramework.UnityOfWork
{
    public interface IUoWFlightService : IDisposable
    {
        IDbContextTransaction BeginTransaction();

        #region ENTITIES

        IRepositoryBase<City> City { get; }

        IRepositoryBase<Airport> Airport { get; }

        #endregion

        void ExecuteSqlRaw(string sqlCommand);
        Task<int> CommitAsync();
        int Commit();
    }
}