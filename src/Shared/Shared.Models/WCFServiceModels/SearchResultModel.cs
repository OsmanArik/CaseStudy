namespace Shared.Models.WCFServiceModels
{
    public class SearchResultModel
    {
        public bool HasError { get; set; }
        public List<FlightOptionModel> FlightOptions { get; set; } = new List<FlightOptionModel>();
    }
}