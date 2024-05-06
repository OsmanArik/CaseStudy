using FlightService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightService.DataAccess.EntityFramework.Configurations
{
    public class AirportConfiguration : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.IATACode)
                .HasMaxLength(4)
                .IsRequired();

            builder.HasOne(x => x.City)
                .WithMany(x => x.Airports)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}