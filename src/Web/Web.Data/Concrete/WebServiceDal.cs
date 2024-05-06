using Newtonsoft.Json;
using Shared.Core.Utilies.Results;
using Shared.Models.BusinessModel;
using Shared.Models.WCFServiceModels;
using Web.Data.Abstract;

namespace Web.Data.Concrete
{
    public class WebServiceDal : WebServiceClientBase, IWebServiceDal
    {
        #region Constructor

        public WebServiceDal() { }

        #endregion

        #region Methods

        #region Public Methods

        public IResult<object> GetAirports()
        {
            var data = new List<Object>();

            var response = this.Get<ResultModel<List<AirportDataModel>>>("GetAirports");

            if (response != null && response.Data.Count > 0)
            {
                var result = (from item in response.Data
                              select new
                              {
                                  id = item.IATACode,
                                  text = item.AirportName,
                                  disabled = false
                              }).ToList();

                data.AddRange(result);
            }

            return new ResultModel<object>(data);
        }

        public IResult<List<FlightOptionModel>> GetSearch(SearchRequestModel model)
        {
            var data = new List<FlightOptionModel>();

            var requestString = JsonConvert.SerializeObject(model);

            var response = this.Post<ResultModel<List<FlightOptionModel>>>("GetSearch", requestString);

            if (response?.Data != null) data = response.Data;

            return new ResultModel<List<FlightOptionModel>>(data);
        }

        #endregion

        #endregion

    }
}