using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Models.WCFServiceModels;
using System.Diagnostics;
using Web.Business.Abstract;
using Web.UI.Extensions;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBusinessWebService _businessWebService;

        public HomeController(ILogger<HomeController> logger, IBusinessWebService businessWebService)
        {
            _logger = logger;
            _businessWebService = businessWebService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAirports()
        {
            var data = _businessWebService.GetAirports();

            return Json(data?.Data);
        }

        [HttpPost]
        public async Task<JsonResult> GetSearch(SearchRequestModel model)
        {
            try
            {
                var result = await Task.Run(async () =>
                {
                    var data = _businessWebService.GetSearch(model).Data;

                    var html = await this.RenderViewAsync("_FlightSearch", data, true);

                    return new AjaxReturnModel
                    {
                        Success = true,
                        Data = html,
                    };
                });

                return Json(result);
            }
            catch (Exception e)
            {
                return Json(new AjaxReturnModel { Success = false, Message = e.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}