using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TradeApp.Business.BaseMetaModels;
using TradeApp.Data.Models.BaseMetaDbModels;
using TradeApp.UI.Options;

namespace TradeApp.UI.Controllers
{
    public class ServersController : Controller
    {
        private readonly IOptions<ApiOptions> _options;

        public ServersController(IOptions<ApiOptions> options)
        {
            _options = options;
        }

        // GET: Servers
        public IActionResult Index()
        {
            var response = ApiConsumer.Get<List<Server>>(_options.Value.ApiUrl + "server");

            return View(response.Data);
        }

        // GET: Servers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =
                ApiConsumer.Get<List<GroupCrossReferenceResponse>>(_options.Value.ApiUrl + $"group/server/{id}");

            return View(response.Data);
        }
    }
}