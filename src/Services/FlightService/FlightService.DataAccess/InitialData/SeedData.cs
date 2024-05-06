using FlightService.DataAccess.EntityFramework.Context;
using FlightService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shared.Core.Utilies.Configuration;

namespace FlightService.DataAccess.InitialData
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FlightServiceContext(serviceProvider.GetRequiredService<DbContextOptions<FlightServiceContext>>()))
            {
                var configuration = ConfigurationService.GetConfiguration();

                if (!context.Cities.Any())
                {
                    var citiesJsonPath = configuration["SeedData:CitiesJsonPath"];

                    var citiesJson = File.ReadAllText(citiesJsonPath);

                    var cities = JsonConvert.DeserializeObject<List<City>>(citiesJson);

                    context.Cities.AddRange(cities);

                    context.SaveChanges();
                }

                if (!context.Airports.Any())
                {
                    var airportsJsonPath = configuration["SeedData:AirportJsonPath"];

                    var airportsJson = File.ReadAllText(airportsJsonPath);

                    var airports = JsonConvert.DeserializeObject<List<Airport>>(airportsJson);

                    context.Airports.AddRange(airports);

                    context.SaveChanges();
                }
            }
        }
    }
}