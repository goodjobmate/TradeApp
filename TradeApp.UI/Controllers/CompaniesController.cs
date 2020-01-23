using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.UI.Controllers
{
    public class CompaniesController : Controller
    {
        // GET: Companies
        public IActionResult Index()
        {
            var response = ApiConsumer.Get<List<Company>>("https://localhost:44305/api/company");
            return View(response.Data);
        }

        // GET: Companies/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Company>($"https://localhost:44305/api/company/{id}");

            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description,IsEnable")] Company company)
        {
            if (ModelState.IsValid)
            {
                ApiConsumer.Post<Company>(company, "https://localhost:44305/api/company");

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Company>($"https://localhost:44305/api/company/{id}");
            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description,IsEnable")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ApiConsumer.Put<Company>(company, $"https://localhost:44305/api/company/{id}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        // GET: Companies/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Company>($"https://localhost:44305/api/company/{id}");

            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: Companies/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ApiConsumer.Delete<Company>($"https://localhost:44305/api/company/{id}");
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return ApiConsumer.Get<Company>($"https://localhost:44305/api/Company/{id}").Data != null;
        }
    }
}