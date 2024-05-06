using FlightService.API.Controllers.Base;
using FlightService.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Utilies.Results;
using Shared.Models.BusinessModel;
using Shared.Models.WCFServiceModels;    

namespace FlightService.API.Controllers
{
    public class FlightController : BaseApiController
    {
        #region Variable

        private readonly ILogger<FlightController> _logger;

        private readonly IBusinessService _businessService;

        #endregion

        #region Constructor

        public FlightController(ILogger<FlightController> logger, IBusinessService businessService)
        {
            _logger = logger;
            _businessService = businessService;
        }

        #endregion

        #region Methods

        #region Public Methods

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel<AirportDataModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("GetAirports")]
        public IActionResult AirportList()
        {
            var result = _businessService.GetAirportData();

            return StatusResult(result);
        }

        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel<List<FlightOptionModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("GetSearch")]
        public async Task<IActionResult> GetSearch(SearchRequestModel searchRequestModel)
        {
            var result = await _businessService.SearchAsync(searchRequestModel);

            return StatusResult(result);
        }

        #endregion

        #endregion

    }
}