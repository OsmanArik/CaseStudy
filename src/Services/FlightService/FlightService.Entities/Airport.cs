using Shared.Core.Domain.Entities;

namespace FlightService.Entities
{
    public class Airport : EntityBase<int>, IEntity, ISoftDelete
    {
        public string Name { get; set; }
        public string IATACode { get; set; }
        public bool IsDeleted { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
    }
}