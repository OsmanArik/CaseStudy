using Shared.Core.Utilies.Results;
using Shared.Models.WCFServiceModels;

namespace Web.Business.Abstract
{
    public interface IBusinessWebService
    {
        IResult<object> GetAirports();

        IResult<List<FlightOptionModel>> GetSearch(SearchRequestModel model);
    }
}