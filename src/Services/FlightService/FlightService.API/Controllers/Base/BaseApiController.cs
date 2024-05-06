using Microsoft.AspNetCore.Mvc;
using Shared.Core.Utilies.Results;

namespace FlightService.API.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult StatusResult<T>(ResultModel<T> data)
        {
            return StatusCode((int)data.StatusCode, data);
        }

        protected IActionResult StatusResult(ResultModel data)
        {
            return StatusCode((int)data.StatusCode, data);
        }
    }
}