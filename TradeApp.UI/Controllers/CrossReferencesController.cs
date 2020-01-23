using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.UI.Controllers
{
    public class CrossReferencesController : Controller
    {
        // GET: CrossReferences
        public IActionResult Index()
        {
            var response = ApiConsumer.Get<List<CrossReference>>("https://localhost:44305/api/CrossReference");

            foreach (var crossReference in response.Data)
            {
                var detail =
                    ApiConsumer.Get<CrossReference>(
                        $"https://localhost:44305/api/CrossReference/{crossReference.Id}/detail");

                crossReference.Server = detail.Data.Server;
                crossReference.Regulation = detail.Data.Regulation;
                crossReference.Branch = detail.Data.Branch;
                crossReference.Company = detail.Data.Company;
            }

            return View(response.Data);
        }

        // GET: CrossReferences/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =
                ApiConsumer.Get<CrossReference>($"https://localhost:44305/api/CrossReference/{id}/detail");

            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // GET: CrossReferences/Create
        public IActionResult Create()
        {
            var server = ApiConsumer.Get<List<Server>>("https://localhost:44305/api/server");
            var regulation = ApiConsumer.Get<List<Regulation>>("https://localhost:44305/api/regulation");
            var branch = ApiConsumer.Get<List<Branch>>("https://localhost:44305/api/branch");
            var company = ApiConsumer.Get<List<Company>>("https://localhost:44305/api/company");

            ViewData["BranchId"] = new SelectList(branch.Data, "Id", "Name");
            ViewData["CompanyId"] = new SelectList(company.Data, "Id", "Name");
            ViewData["RegulationId"] = new SelectList(regulation.Data, "Id", "Name");
            ViewData["ServerId"] = new SelectList(server.Data, "Id", "Name");

            return View();
        }

        // POST: CrossReferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ServerId,RegulationId,BranchId,CompanyId")]
            CrossReference crossReference)
        {
            if (ModelState.IsValid)
            {
                ApiConsumer.Post<Branch>(crossReference, "https://localhost:44305/api/CrossReference");
                return RedirectToAction(nameof(Index));
            }

            var server = ApiConsumer.Get<List<Server>>("https://localhost:44305/api/server");
            var regulation = ApiConsumer.Get<List<Regulation>>("https://localhost:44305/api/regulation");
            var branch = ApiConsumer.Get<List<Branch>>("https://localhost:44305/api/branch");
            var company = ApiConsumer.Get<List<Company>>("https://localhost:44305/api/company");

            ViewData["BranchId"] = new SelectList(branch.Data, "Id", "Name", crossReference.BranchId);
            ViewData["CompanyId"] = new SelectList(company.Data, "Id", "Name", crossReference.CompanyId);
            ViewData["RegulationId"] = new SelectList(regulation.Data, "Id", "Name", crossReference.RegulationId);
            ViewData["ServerId"] = new SelectList(server.Data, "Id", "Name", crossReference.ServerId);
            return View(crossReference);
        }

        // GET: CrossReferences/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var response =
                ApiConsumer.Get<CrossReference>($"https://localhost:44305/api/CrossReference/{id}");

            if (response.Data == null)
            {
                return NotFound();
            }

            var server = ApiConsumer.Get<List<Server>>("https://localhost:44305/api/server");
            var regulation = ApiConsumer.Get<List<Regulation>>("https://localhost:44305/api/regulation");
            var branch = ApiConsumer.Get<List<Branch>>("https://localhost:44305/api/branch");
            var company = ApiConsumer.Get<List<Company>>("https://localhost:44305/api/company");

            ViewData["BranchId"] = new SelectList(branch.Data, "Id", "Name", response.Data.BranchId);
            ViewData["CompanyId"] = new SelectList(company.Data, "Id", "Name", response.Data.CompanyId);
            ViewData["RegulationId"] = new SelectList(regulation.Data, "Id", "Name", response.Data.RegulationId);
            ViewData["ServerId"] = new SelectList(server.Data, "Id", "Name", response.Data.ServerId);
            return View(response.Data);
        }

        // POST: CrossReferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ServerId,RegulationId,BranchId,CompanyId")]
            CrossReference crossReference)
        {
            if (id != crossReference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ApiConsumer.Put<Branch>(crossReference, $"https://localhost:44305/api/CrossReference/{id}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrossReferenceExists(crossReference.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            var response =
                ApiConsumer.Get<CrossReference>($"https://localhost:44305/api/CrossReference/{id}");

            var server = ApiConsumer.Get<List<Server>>("https://localhost:44305/api/server");
            var regulation = ApiConsumer.Get<List<Regulation>>("https://localhost:44305/api/regulation");
            var branch = ApiConsumer.Get<List<Branch>>("https://localhost:44305/api/branch");
            var company = ApiConsumer.Get<List<Company>>("https://localhost:44305/api/company");

            ViewData["BranchId"] = new SelectList(branch.Data, "Id", "Name", response.Data.BranchId);
            ViewData["CompanyId"] = new SelectList(company.Data, "Id", "Name", response.Data.CompanyId);
            ViewData["RegulationId"] = new SelectList(regulation.Data, "Id", "Name", response.Data.RegulationId);
            ViewData["ServerId"] = new SelectList(server.Data, "Id", "Name", response.Data.ServerId);

            return View(crossReference);
        }

        // GET: CrossReferences/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =
                ApiConsumer.Get<CrossReference>($"https://localhost:44305/api/CrossReference/{id}/detail");

            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: CrossReferences/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ApiConsumer.Delete<CrossReference>($"https://localhost:44305/api/CrossReference/{id}");
            return RedirectToAction(nameof(Index));
        }

        private bool CrossReferenceExists(int id)
        {
            return ApiConsumer.Get<CrossReference>($"https://localhost:44305/api/CrossReference/{id}").Data != null;
        }
    }
}