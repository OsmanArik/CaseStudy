namespace Shared.Models.WCFServiceModels
{
    public class SearchRequestModel
    {
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; } = null;
    }
}