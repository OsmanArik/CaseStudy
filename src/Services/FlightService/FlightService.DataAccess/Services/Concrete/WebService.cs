using FlightService.DataAccess.Services.Abstraction;
using Shared.Core.SOAP.Concrete;
using Shared.Core.SOAP.Models;
using Shared.Core.Utilies.Configuration;
using Shared.Models.WCFServiceModels;

namespace FlightService.DataAccess.Services.Concrete
{
    public class WebService : IWebService
    {
        #region Constructor

        public WebService() { }

        #endregion

        #region Methods

        #region Public Methods

        public async Task<string> GetAirSearch(SearchRequestModel searchRequest)
        {
            try
            {
                var model = new SearchResultModel();

                var response = await SendRequest(searchRequest);

                if (string.IsNullOrEmpty(response)) throw new Exception("Istek gönderilemedi!");

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Methods

        private async Task<string> SendRequest(SearchRequestModel model)
        {
            try
            {
                string result = string.Empty;

                using (SOAPClient soapClient = new SOAPClient())
                {
                    var configuration = ConfigurationService.GetConfiguration();

                    var request = CreateRequest(model);

                    var requestSoap = soapClient.RequestSchemaCreate(request);

                    var requestParameterModel = new RequestParameterModel
                    {
                        Url = configuration["Services:FlightProviderUrl"],
                        Request = requestSoap,
                        SoapActionName = "http://tempuri.org/IAirSearch/AvailabilitySearch"
                    };

                    var response = await soapClient.SendRequestAsync(requestParameterModel);

                    result = soapClient.ReadResponse(response);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string CreateRequest(SearchRequestModel searchRequest)
            => $@"
                {(searchRequest.ArrivalDate != null ? $"<flig:ArrivalDate>{searchRequest.ArrivalDate.Value.DateTimeFormatEx()}</flig:ArrivalDate>" : String.Empty)}
                <flig:DepartureDate>{searchRequest.DepartureDate.DateTimeFormatEx()}</flig:DepartureDate>
                <flig:Destination>{searchRequest.Destination}</flig:Destination>
                <flig:Origin>{searchRequest.Origin}</flig:Origin>";

        #endregion

        #endregion

    }
}
