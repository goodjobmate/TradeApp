using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TradeApp.Business.WidgetModels;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;
using TradeApp.Data.Models.TradeDbModels;
using TradeApp.UI.Models;
using TradeApp.UI.Options;

namespace TradeApp.UI.Controllers
{
    public class TagController : Controller
    {
        private readonly IOptions<ApiOptions> _options;

        public TagController(IOptions<ApiOptions> options)
        {
            _options = options;
        }

        public IActionResult Index()
        {
            var server = ApiConsumer.Get<List<Server>>(_options.Value.ApiUrl + "server");
            var regulation = ApiConsumer.Get<List<Regulation>>(_options.Value.ApiUrl + "regulation");

            regulation.Data.Add(new Regulation{Id = 0, Name = "Select (optional)"});

            ViewData["RegulationId"] = new SelectList(regulation.Data, "Id", "Name", 0);
            ViewData["ServerId"] = new SelectList(server.Data, "Id", "Name");

            return View();
        }

        public IActionResult List([Bind("ServerId, RegulationId")] GetTagsRequestViewModel request)
        {
            var reg = request.RegulationId == 0 ? "" : request.RegulationId.ToString();
            var response =
                ApiConsumer.Get<IEnumerable<ResultDto>>(
                    _options.Value.ApiUrl + $"tag?serverId={request.ServerId}&regulationId={reg}");

            return View(response.Data);
        }

        // GET: Tag/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =
                ApiConsumer.Get<Tag>(
                    _options.Value.ApiUrl + $"tag/{id}");

            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // GET: Tag/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Key,Logins")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                var response =
                    ApiConsumer.Post<Tag>(tag, _options.Value.ApiUrl + "tag/v2");
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tag/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =
                ApiConsumer.Get<Tag>(
                    _options.Value.ApiUrl + $"tag/{id}");

            if (response.Data == null)
            {
                return NotFound();
            }
            return View(response.Data);
        }

        // POST: Tag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Name,Key,Logins,Id")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response =
                        ApiConsumer.Put<Tag>(tag, _options.Value.ApiUrl + "tag/v2");
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tag/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =
                ApiConsumer.Get<Tag>(
                    _options.Value.ApiUrl + $"tag/{id}");

            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response =
                ApiConsumer.Delete<Tag>(
                    _options.Value.ApiUrl + $"tag/v2/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
