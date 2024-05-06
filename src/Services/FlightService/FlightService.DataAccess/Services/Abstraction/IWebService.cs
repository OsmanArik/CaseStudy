using Shared.Models.WCFServiceModels;

namespace FlightService.DataAccess.Services.Abstraction
{
    public interface IWebService
    {
        Task<string> GetAirSearch(SearchRequestModel searchRequest);
    }
}