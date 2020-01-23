using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeApp.Business.BaseMetaModels;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.UI.Controllers
{
    public class GroupCrossReferencesController : Controller
    {
        // GET: GroupCrossReferences
        public IActionResult Index()
        {
            var response =
                ApiConsumer.Get<List<GroupCrossReference>>("https://localhost:44305/api/Group");
            return View(response.Data);
        }

        // GET: GroupCrossReferences/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =
                ApiConsumer.Get<GroupCrossReference>($"https://localhost:44305/api/Group/{id}");

            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // GET: GroupCrossReferences/Create
        public IActionResult Create()
        {
            var response =
                ApiConsumer.Get<List<CrossReference>>("https://localhost:44305/api/CrossReference");

            ViewData["CrossReferenceId"] = new SelectList(response.Data, "Id", "Id");
            return View();
        }

        // POST: GroupCrossReferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CrossReferenceId,GroupName")]
            GroupCrossReference groupCrossReference)
        {
            if (ModelState.IsValid)
            {
                ApiConsumer.Post<CrossReference>(groupCrossReference,
                    "https://localhost:44305/api/Group");

                return RedirectToAction(nameof(Index));
            }

            var crossReferenceResponse =
                ApiConsumer.Get<List<CrossReference>>("https://localhost:44305/api/CrossReference");

            ViewData["CrossReferenceId"] =
                new SelectList(crossReferenceResponse.Data, "Id", "Id", groupCrossReference.CrossReferenceId);

            return View(groupCrossReference);
        }

        // GET: GroupCrossReferences/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gcr =
                ApiConsumer.Get<GroupCrossReference>($"https://localhost:44305/api/Group/{id}");

            var crossReference =
                ApiConsumer.Get<CrossReference>(
                    $"https://localhost:44305/api/CrossReference/{gcr.Data.CrossReferenceId}");
            
            var response = ApiConsumer.Get<List<GroupCrossReferenceResponse>>(
                $"https://localhost:44305/api/group/server/{crossReference.Data.ServerId}");

            var server = ApiConsumer.Get<List<Server>>("https://localhost:44305/api/server");
            var regulation = ApiConsumer.Get<List<Regulation>>("https://localhost:44305/api/regulation");
            var branch = ApiConsumer.Get<List<Branch>>("https://localhost:44305/api/branch");
            var company = ApiConsumer.Get<List<Company>>("https://localhost:44305/api/company");

            ViewData["ServerName"] = new SelectList(server.Data, "Id", "Name", crossReference.Data.ServerId);
            ViewData["RegulationName"] = new SelectList(regulation.Data, "Id", "Name", crossReference.Data.RegulationId);
            ViewData["BranchName"] = new SelectList(branch.Data, "Id", "Name", crossReference.Data.BranchId);
            ViewData["CompanyName"] = new SelectList(company.Data, "Id", "Name", crossReference.Data.CompanyId);

            return View(response.Data);
        }

        // POST: GroupCrossReferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, GroupCrossReferenceResponse groupCrossReference)
        {
            if (id != groupCrossReference.GroupCrossReferenceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response =
                        ApiConsumer.Get<CrossReference>(
                            "https://localhost:44305/api/CrossReference" +
                            $"?serverId={groupCrossReference.ServerId}&regulationId={groupCrossReference.RegulationId}" +
                            $"&branchId={groupCrossReference.BranchId}&companyI={groupCrossReference.CompanyId}");

                    if (response.Data == null)
                    {
                        return NotFound();
                    }

                    var gcr = new GroupCrossReference
                    {
                        CrossReferenceId = response.Data.Id, GroupName = groupCrossReference.GroupName
                    };

                    ApiConsumer.Put<GroupCrossReference>(gcr, "https://localhost:44305/api/Group");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupCrossReferenceExists(groupCrossReference.GroupCrossReferenceId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            var server = ApiConsumer.Get<List<Server>>("https://localhost:44305/api/server");
            var regulation = ApiConsumer.Get<List<Regulation>>("https://localhost:44305/api/regulation");
            var branch = ApiConsumer.Get<List<Branch>>("https://localhost:44305/api/branch");
            var company = ApiConsumer.Get<List<Company>>("https://localhost:44305/api/company");

            ViewData["ServerName"] = new SelectList(server.Data, "Id", "Name", groupCrossReference.ServerId);
            ViewData["RegulationName"] =
                new SelectList(regulation.Data, "Id", "Name", groupCrossReference.RegulationId);
            ViewData["BranchName"] = new SelectList(branch.Data, "Id", "Name", groupCrossReference.BranchId);
            ViewData["CompanyName"] = new SelectList(company.Data, "Id", "Name", groupCrossReference.CompanyId);

            return View(groupCrossReference);
        }

        // GET: GroupCrossReferences/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =
                ApiConsumer.Get<GroupCrossReference>($"https://localhost:44305/api/Group/{id}");
            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: GroupCrossReferences/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ApiConsumer.Delete<GroupCrossReference>($"https://localhost:44305/api/Group/{id}");
            return RedirectToAction(nameof(Index));
        }

        private bool GroupCrossReferenceExists(int id)
        {
            return ApiConsumer.Get<GroupCrossReference>($"https://localhost:44305/api/Group/{id}").Data !=
                   null;
        }
    }
}