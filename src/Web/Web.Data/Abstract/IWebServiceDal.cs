using Shared.Core.Utilies.Results;
using Shared.Models.WCFServiceModels;

namespace Web.Data.Abstract
{
    public interface IWebServiceDal
    {
        IResult<object> GetAirports();

        IResult<List<FlightOptionModel>> GetSearch(SearchRequestModel model);
    }
}