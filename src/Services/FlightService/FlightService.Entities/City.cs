using Shared.Core.Domain.Entities;

namespace FlightService.Entities
{
    public class City : EntityBase<int>, IEntity, ISoftDelete
    {
        public City()
        {
            this.Airports = new HashSet<Airport>();
        }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Airport> Airports { get; set; }
    }
}