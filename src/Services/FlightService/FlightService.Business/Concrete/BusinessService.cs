using AutoMapper;
using FlightService.Business.Abstract;
using FlightService.Business.ValidationRules.FluentValidation;
using FlightService.DataAccess.EntityFramework.UnityOfWork;
using FlightService.DataAccess.Services.Abstraction;
using FlightService.Entities;
using Shared.Core.Aspects.Autofact.Validation;
using Shared.Core.Middleware.Caching;
using Shared.Core.Utilies.Results;
using Shared.Models.BusinessModel;
using Shared.Models.WCFServiceModels;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace FlightService.Business.Concrete
{
    public class BusinessService : IBusinessService
    {
        #region Variable

        private readonly IUoWFlightService _repository;
        private readonly IWebService _service;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public BusinessService(IUoWFlightService repository, IWebService service, ICacheManager cacheManager, IMapper mapper)
        {
            _repository = repository;
            _service = service;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        #region Public Methods

        public ResultModel<List<AirportDataModel>> GetAirportData()
        {
            var isAlreadyExist = _cacheManager.IsAdd(CacheKeys.AirportList);

            if (isAlreadyExist)
            {
                var cachedData = _cacheManager.Get<List<AirportDataModel>>(CacheKeys.AirportList);

                return new ResultModel<List<AirportDataModel>> { Data = cachedData };
            }

            var data = (from airport in _repository.Airport.GetAll()
                       select new AirportDataModel
                       {
                           AirportName= airport.Name,
                           IATACode = airport.IATACode
                       }).ToList();

            _cacheManager.Add(CacheKeys.AirportList, data, 60);

            return new ResultModel<List<AirportDataModel>>() { Data = _mapper.Map<List<AirportDataModel>>(data) };
        }

        [ValidationAspect(typeof(SearchRequestValidator))]
        public async Task<ResultModel<List<FlightOptionModel>>> SearchAsync(SearchRequestModel searchRequestModel)
        {
            try
            {
                var data = await _service.GetAirSearch(searchRequestModel);

                if (string.IsNullOrEmpty(data))
                    return new ResultModel<List<FlightOptionModel>>
                    {
                        IsSuccess = false,
                        Errors = new List<string>() { "No data found!" }
                    };

                var readResponse = ReadSoapResponse(data);

                return new ResultModel<List<FlightOptionModel>>() { Data = readResponse };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<FlightOptionModel> ReadSoapResponse(string data)
        {
            try
            {
                var model = new List<FlightOptionModel>();

                XElement xElement = XElement.Parse(data);

                XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());

                nsm.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
                nsm.AddNamespace("a", "http://schemas.datacontract.org/2004/07/FlightProvider");

                bool hasError = Boolean.TryParse(xElement.XPathSelectElement("/a:HasError", nsm).Value, out bool isError) ? isError : false;

                model = (from flightOptionModel in xElement.XPathSelectElements("//a:FlightOption", nsm)
                         select new FlightOptionModel
                         {
                             DepartureDateTime = DateTime.TryParse(flightOptionModel.XPathSelectElement("a:DepartureDateTime", nsm)?.Value, out DateTime resultDepartureDatetime) ? resultDepartureDatetime : DateTime.MinValue,
                             ArrivalDateTime = DateTime.TryParse(flightOptionModel.XPathSelectElement("a:ArrivalDateTime", nsm)?.Value, out DateTime resultArrivalDatetime) ? resultArrivalDatetime : DateTime.MinValue,
                             FlightNumber = flightOptionModel.XPathSelectElement("a:FlightNumber", nsm)?.Value,
                             IsRoundTrip = Boolean.TryParse(flightOptionModel.XPathSelectElement("a:IsRoundTrip", nsm)?.Value, out bool resultIsRoundTrip) ? resultIsRoundTrip : false,
                             Price = decimal.Parse(flightOptionModel.XPathSelectElement("a:Price", nsm)?.Value, System.Globalization.CultureInfo.InvariantCulture),
                             DestinationPoint= flightOptionModel.XPathSelectElement("a:DestinationPoint", nsm)?.Value,
                             OriginPoint= flightOptionModel.XPathSelectElement("a:OriginPoint", nsm)?.Value
                         }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

    }
}