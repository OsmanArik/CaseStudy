using FlightService.DataAccess.EntityFramework.Configurations;
using FlightService.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Utilies.Configuration;
using Microsoft.Extensions.Configuration;

namespace FlightService.DataAccess.EntityFramework.Context
{
    public class FlightServiceContext : DbContext
    {
        #region Variable
        #endregion

        #region Constructor

        public FlightServiceContext(DbContextOptions<FlightServiceContext> options) : base(options) { }

        #endregion

        #region Entities

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Airport> Airports { get; set; }

        #endregion

        #region Methods

        #region Protected Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = ConfigurationService.GetConfiguration();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("FlightServiceConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(60));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new AirportConfiguration());
        }

        #endregion

        #endregion

    }
}