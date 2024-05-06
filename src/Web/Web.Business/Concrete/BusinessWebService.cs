using Shared.Core.Aspects.Autofact.Validation;
using Shared.Core.Utilies.Results;
using Shared.Models.WCFServiceModels;
using Web.Business.Abstract;
using Web.Business.ValidationRules.FluentValidation;
using Web.Data.Abstract;

namespace Web.Business.Concrete
{
    public class BusinessWebService : IBusinessWebService
    {
        #region Variable

        private readonly IWebServiceDal _webServiceDal;

        #endregion

        #region Constructor

        public BusinessWebService(IWebServiceDal webServiceDal)
        {
            _webServiceDal = webServiceDal;
        }

        #endregion

        #region Methods

        #region Public Methods

        public IResult<object> GetAirports()
        {
            var data = _webServiceDal.GetAirports();

            return data;
        }

        [ValidationAspect(typeof(SearchRequestValidator))]
        public IResult<List<FlightOptionModel>> GetSearch(SearchRequestModel model)
        {
            var data = _webServiceDal.GetSearch(model);

            return data;
        }

        #endregion

        #endregion

    }
}