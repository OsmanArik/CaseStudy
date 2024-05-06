namespace Shared.Models.WCFServiceModels
{
    public class FlightOptionModel
    {
        public string DestinationPoint { get; set; }
        public string OriginPoint { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public decimal Price { get; set; }
        public bool IsRoundTrip { get; set; }
    }
}