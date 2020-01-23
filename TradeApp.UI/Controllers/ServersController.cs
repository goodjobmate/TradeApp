using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TradeApp.Business.BaseMetaModels;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.UI.Controllers
{
    public class ServersController : Controller
    {
        // GET: Servers
        public IActionResult Index()
        {
            var response = ApiConsumer.Get<List<Server>>("https://localhost:44305/api/server");

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
                ApiConsumer.Get<List<GroupCrossReferenceResponse>>($"https://localhost:44305/api/group/server/{id}");

            return View(response.Data);
        }
    }
}