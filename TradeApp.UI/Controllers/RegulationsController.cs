using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.UI.Controllers
{
    public class RegulationsController : Controller
    {
        // GET: Regulations
        public IActionResult Index()
        {
            var response = ApiConsumer.Get<List<Regulation>>("https://localhost:44305/api/regulation");
            return View(response.Data);
        }

        // GET: Regulations/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Regulation>($"https://localhost:44305/api/regulation/{id}");
            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // GET: Regulations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Regulations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description")] Regulation regulation)
        {
            if (ModelState.IsValid)
            {
                ApiConsumer.Post<Regulation>(regulation, "https://localhost:44305/api/regulation");
                return RedirectToAction(nameof(Index));
            }

            return View(regulation);
        }

        // GET: Regulations/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Regulation>($"https://localhost:44305/api/regulation/{id}");
            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: Regulations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description")] Regulation regulation)
        {
            if (id != regulation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ApiConsumer.Put<List<Regulation>>(regulation, "https://localhost:44305/api/regulation");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegulationExists(regulation.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(regulation);
        }

        // GET: Regulations/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Regulation>($"https://localhost:44305/api/regulation/{id}");
            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: Regulations/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ApiConsumer.Delete<Regulation>($"https://localhost:44305/api/regulation/{id}");
            return RedirectToAction(nameof(Index));
        }

        private bool RegulationExists(int id)
        {
            return ApiConsumer.Get<Regulation>($"https://localhost:44305/api/Regulation/{id}").Data != null;
        }
    }
}