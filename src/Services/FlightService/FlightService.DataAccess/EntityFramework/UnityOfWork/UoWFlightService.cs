using FlightService.DataAccess.Base;
using FlightService.DataAccess.EntityFramework.Context;
using FlightService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Core.Domain.Repositories;

namespace FlightService.DataAccess.EntityFramework.UnityOfWork
{
    public class UoWFlightService : IUoWFlightService
    {
        public FlightServiceContext _dbContext { get; }

        public UoWFlightService(FlightServiceContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region ENTITIES

        public IRepositoryBase<City> City => new EfRepositoryBase<City>(_dbContext);

        public IRepositoryBase<Airport> Airport => new EfRepositoryBase<Airport>(_dbContext);

        #endregion

        public void ExecuteSqlRaw(string sqlCommand)
        {
            _dbContext.Database.SetCommandTimeout(new TimeSpan(0, 2, 0));
            _dbContext.Database.ExecuteSqlRaw(sqlCommand);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}