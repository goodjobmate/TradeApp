using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;
using TradeApp.UI.Options;

namespace TradeApp.UI.Controllers
{
    public class CrossReferencesController : Controller
    {
        private readonly IOptions<ApiOptions> _options;

        public CrossReferencesController(IOptions<ApiOptions> options)
        {
            _options = options;
        }

        // GET: CrossReferences
        public IActionResult Index()
        {
            var response = ApiConsumer.Get<List<CrossReference>>(_options.Value.ApiUrl + "CrossReference");

            foreach (var crossReference in response.Data)
            {
                var detail =
                    ApiConsumer.Get<CrossReference>(
                        _options.Value.ApiUrl + $"CrossReference/{crossReference.Id}/detail");

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
                ApiConsumer.Get<CrossReference>(_options.Value.ApiUrl + $"CrossReference/{id}/detail");

            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // GET: CrossReferences/Create
        public IActionResult Create()
        {
            var server = ApiConsumer.Get<List<Server>>(_options.Value.ApiUrl + "server");
            var regulation = ApiConsumer.Get<List<Regulation>>(_options.Value.ApiUrl + "regulation");
            var branch = ApiConsumer.Get<List<Branch>>(_options.Value.ApiUrl + "branch");
            var company = ApiConsumer.Get<List<Company>>(_options.Value.ApiUrl + "company");

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
                ApiConsumer.Post<Branch>(crossReference, _options.Value.ApiUrl + "CrossReference");
                return RedirectToAction(nameof(Index));
            }

            var server = ApiConsumer.Get<List<Server>>(_options.Value.ApiUrl + "server");
            var regulation = ApiConsumer.Get<List<Regulation>>(_options.Value.ApiUrl + "regulation");
            var branch = ApiConsumer.Get<List<Branch>>(_options.Value.ApiUrl + "branch");
            var company = ApiConsumer.Get<List<Company>>(_options.Value.ApiUrl + "company");

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
                ApiConsumer.Get<CrossReference>(_options.Value.ApiUrl + $"CrossReference/{id}");

            if (response.Data == null)
            {
                return NotFound();
            }

            var server = ApiConsumer.Get<List<Server>>(_options.Value.ApiUrl + "server");
            var regulation = ApiConsumer.Get<List<Regulation>>(_options.Value.ApiUrl + "regulation");
            var branch = ApiConsumer.Get<List<Branch>>(_options.Value.ApiUrl + "branch");
            var company = ApiConsumer.Get<List<Company>>(_options.Value.ApiUrl + "company");

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
                    ApiConsumer.Put<Branch>(crossReference, _options.Value.ApiUrl + $"CrossReference/{id}");
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
                ApiConsumer.Get<CrossReference>(_options.Value.ApiUrl + $"CrossReference/{id}");

            var server = ApiConsumer.Get<List<Server>>(_options.Value.ApiUrl + "server");
            var regulation = ApiConsumer.Get<List<Regulation>>(_options.Value.ApiUrl + "regulation");
            var branch = ApiConsumer.Get<List<Branch>>(_options.Value.ApiUrl + "branch");
            var company = ApiConsumer.Get<List<Company>>(_options.Value.ApiUrl + "company");

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
                ApiConsumer.Get<CrossReference>(_options.Value.ApiUrl + $"CrossReference/{id}/detail");

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
            ApiConsumer.Delete<CrossReference>(_options.Value.ApiUrl + $"CrossReference/{id}");
            return RedirectToAction(nameof(Index));
        }

        private bool CrossReferenceExists(int id)
        {
            return ApiConsumer.Get<CrossReference>(_options.Value.ApiUrl + $"CrossReference/{id}").Data != null;
        }
    }
}