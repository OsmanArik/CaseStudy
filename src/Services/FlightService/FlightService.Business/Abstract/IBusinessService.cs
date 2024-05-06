using Shared.Core.Utilies.Results;
using Shared.Models.BusinessModel;
using Shared.Models.WCFServiceModels;

namespace FlightService.Business.Abstract
{
    public interface IBusinessService
    {
        Task<ResultModel<List<FlightOptionModel>>> SearchAsync(SearchRequestModel searchRequestModel);

        ResultModel<List<AirportDataModel>> GetAirportData();
    }
}