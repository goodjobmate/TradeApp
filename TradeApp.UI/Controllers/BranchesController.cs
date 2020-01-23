using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.UI.Controllers
{
    public class BranchesController : Controller
    {
        // GET: Branches
        public IActionResult Index()
        {
            var response = ApiConsumer.Get<List<Branch>>("https://localhost:44305/api/branch");

            return View(response.Data);
        }

        // GET: Branches/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Branch>($"https://localhost:44305/api/branch/{id}");
            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // GET: Branches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                var response = ApiConsumer.Post<Branch>(branch, "https://localhost:44305/api/branch");
                return RedirectToAction(nameof(Index));
            }

            return View(branch);
        }

        // GET: Branches/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Branch>($"https://localhost:44305/api/branch/{id}");
            if (response == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Branch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = ApiConsumer.Put<Branch>(branch, $"https://localhost:44305/api/branch/{id}");
                    branch = response.Data;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(branch);
        }

        // GET: Branches/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Branch>($"https://localhost:44305/api/branch/{id}");


            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: Branches/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = ApiConsumer.Delete<Branch>($"https://localhost:44305/api/branch/{id}");
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return ApiConsumer.Get<Branch>($"https://localhost:44305/api/Branch/{id}").Data != null;
        }
    }
}