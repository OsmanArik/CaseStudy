using RestSharp;
using Shared.Core.Utilies.Configuration;

namespace Web.Data.Abstract
{
    public class WebServiceClientBase : IDisposable
    {
        #region Variable

        protected RestClient RestClient { get; private set; }

        #endregion

        #region Constructor

        public WebServiceClientBase(string baseUrl = null)
        {
            SetRestClient(baseUrl);
        }

        #endregion

        #region Methods

        #region Public Methods

        #region Get Methods

        public TResponse Get<TResponse>(string resource, IDictionary<string, string> parameters = null)
            where TResponse : new()
        {
            var request = new RestRequest(resource, Method.Get);

            if (parameters != null)
                foreach (var param in parameters)
                    request.AddParameter(param.Key, param.Value);

            var response = RestClient.Execute<TResponse>(request);

            return response.Data;
        }

        public async Task<TResponse> GetAsync<TResponse>(string resource, IDictionary<string, string> parameters = null)
            where TResponse : new()
        {
            var request = new RestRequest(resource, Method.Get);

            if (parameters != null)
                foreach (var param in parameters)
                    request.AddParameter(param.Key, param.Value);

            var response = await RestClient.ExecuteAsync<TResponse>(request);

            return response.Data;
        }

        #endregion

        #region Post Methods

        public TResponse Post<TResponse>(string resource, object requestBody) where TResponse : new()
        {
            var request = new RestRequest(resource, Method.Post).AddJsonBody(requestBody);

            var response = RestClient.Execute<TResponse>(request);

            return response.Data;
        }

        public async Task<TResponse> PostAsync<TResponse>(string resource, object requestBody) where TResponse : new()
        {
            var request = new RestRequest(resource, Method.Post).AddJsonBody(requestBody);

            var response = await RestClient.ExecuteAsync<TResponse>(request);

            return response.Data;
        }

        #endregion

        public void Dispose()
        {
            RestClient?.Dispose();
        }

        #endregion

        #region Private Methods

        private void SetRestClient(string baseUrl)
        {
            if (String.IsNullOrEmpty(baseUrl))
            {
                var configuration = ConfigurationService.GetConfiguration();

                var serivceUrl = configuration["BaseServiceUrl"];

                RestClient = new RestClient(serivceUrl);
            }
            else
                RestClient = new RestClient(baseUrl);
        }

        #endregion

        #endregion

    }
}