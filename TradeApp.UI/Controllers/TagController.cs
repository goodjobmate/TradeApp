using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeApp.Business.WidgetModels;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;
using TradeApp.Data.Models.TradeDbModels;
using TradeApp.UI.Models;

namespace TradeApp.UI.Controllers
{
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            var server = ApiConsumer.Get<List<Server>>("https://localhost:44305/api/server");
            var regulation = ApiConsumer.Get<List<Regulation>>("https://localhost:44305/api/regulation");

            ViewData["RegulationId"] = new SelectList(regulation.Data, "Id", "Name");
            ViewData["ServerId"] = new SelectList(server.Data, "Id", "Name");

            return View();
        }

        public IActionResult List([Bind("ServerId, RegulationId")] GetTagsRequestViewModel request)
        {
            var response =
                ApiConsumer.Get<IEnumerable<ResultDto>>(
                    $"https://localhost:44305/api/tag?serverId={request.ServerId}&regulationId={request.RegulationId}");

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
                    $"https://localhost:44305/api/tag/{id}");

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
                    ApiConsumer.Post<Tag>(tag, "https://localhost:44305/api/tag/v2");
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
                    $"https://localhost:44305/api/tag/{id}");

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
                        ApiConsumer.Put<Tag>(tag, "https://localhost:44305/api/tag/v2");
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
                    $"https://localhost:44305/api/tag/{id}");

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
                    $"https://localhost:44305/api/tag/v2/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
