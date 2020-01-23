using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeApp.Business.BaseMetaModels;
using TradeApp.Data.Models.BaseMetaDbModels;
using TradeApp.UI.Models;

namespace TradeApp.UI.Controllers
{
    public class GroupCrossReferencesController : Controller
    {
        // GET: GroupCrossReferences
        public IActionResult Index()
        {
            var response =
                ApiConsumer.Get<List<GroupCrossReference>>("https://localhost:44305/api/Group/details");

            var viewModel = new List<GroupCrossReferenceViewModel>();

            foreach (var item in response.Data)
            {
                viewModel.Add(new GroupCrossReferenceViewModel
                {
                    Id = item.Id,
                    Name = item.GroupName,
                    CrossReferenceAlias = item.CrossReference.Server.Name + " - " +
                                          item.CrossReference.Regulation.Name + " - " +
                                          item.CrossReference.Branch.Name + " - " +
                                          item.CrossReference.Company.Name
                });
            }

            return View(viewModel);
        }

        // GET: GroupCrossReferences/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =
                ApiConsumer.Get<GroupCrossReference>($"https://localhost:44305/api/Group/{id}/detail");

            if (response.Data == null)
            {
                return NotFound();
            }

            var viewModel = new GroupCrossReferenceViewModel
            {
                Id = response.Data.Id,
                Name = response.Data.GroupName,
                CrossReferenceAlias = response.Data.CrossReference.Server.Name + " - " +
                                      response.Data.CrossReference.Regulation.Name + " - " +
                                      response.Data.CrossReference.Branch.Name + " - " +
                                      response.Data.CrossReference.Company.Name
            };
            return View(viewModel);
        }

        // GET: GroupCrossReferences/Create
        public IActionResult Create()
        {
            var crossReferenceResponse =
                ApiConsumer.Get<List<CrossReference>>("https://localhost:44305/api/CrossReference/details");

            var selectListDataSource = new List<GroupCrossReferenceViewModel>();

            foreach (var item in crossReferenceResponse.Data)
            {
                selectListDataSource.Add(new GroupCrossReferenceViewModel
                {
                    CrossReferenceAlias = item.Server.Name + " - " +
                                          item.Regulation.Name + " - " +
                                          item.Branch.Name + " - " +
                                          item.Company.Name,
                    CrossReferenceId = item.Id
                });
            }

            ViewData["CrossReference"] = new SelectList(selectListDataSource, "CrossReferenceId", "CrossReferenceAlias");
            return View();
        }

        // POST: GroupCrossReferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,CrossReferenceId")]
            GroupCrossReferenceViewModel groupCrossReference)
        {
            if (ModelState.IsValid)
            {
                var objectToCreate = new GroupCrossReference
                {
                    GroupName = groupCrossReference.Name,
                    CrossReferenceId = groupCrossReference.CrossReferenceId
                };

                ApiConsumer.Post<CrossReference>(groupCrossReference,
                    "https://localhost:44305/api/Group");

                return RedirectToAction(nameof(Index));
            }

            var crossReferenceResponse =
                ApiConsumer.Get<List<CrossReference>>("https://localhost:44305/api/CrossReference/details");

            var selectListDataSource = new List<GroupCrossReferenceViewModel>();

            foreach (var item in crossReferenceResponse.Data)
            {
                selectListDataSource.Add(new GroupCrossReferenceViewModel
                {
                    Id = item.Id,
                    CrossReferenceAlias = item.Server.Name + " - " +
                                          item.Regulation.Name + " - " +
                                          item.Branch.Name + " - " +
                                          item.Company.Name,
                    CrossReferenceId = item.Id
                });
            }

            ViewData["CrossReference"] = new SelectList(selectListDataSource, "CrossReferenceId", "CrossReferenceAlias",
                groupCrossReference.CrossReferenceId);

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
                ApiConsumer.Get<GroupCrossReference>($"https://localhost:44305/api/Group/{id}/detail");

            var crossReference = gcr.Data.CrossReference;

            var serverId = crossReference.ServerId;

            var relatedCrossReferences =
                ApiConsumer.Get<List<CrossReference>>($"https://localhost:44305/api/CrossReference/Server/{serverId}");

            var selectListDataSource = new List<GroupCrossReferenceViewModel>();

            foreach (var item in relatedCrossReferences.Data)
            {
                selectListDataSource.Add(new GroupCrossReferenceViewModel
                {
                    Id = item.Id,
                    CrossReferenceAlias = item.Server.Name + " - " +
                                          item.Regulation.Name + " - " +
                                          item.Branch.Name + " - " +
                                          item.Company.Name,
                    CrossReferenceId =  item.Id
                });
            }

            ViewData["CrossReference"] = new SelectList(selectListDataSource, "CrossReferenceId", "CrossReferenceAlias",
                crossReference.Id);

            var viewModel = new GroupCrossReferenceViewModel
            {
                Id = gcr.Data.Id,
                Name = gcr.Data.GroupName,
                CrossReferenceAlias = gcr.Data.CrossReference.Server.Name + " - " +
                                      gcr.Data.CrossReference.Regulation.Name + " - " +
                                      gcr.Data.CrossReference.Branch.Name + " - " +
                                      gcr.Data.CrossReference.Company.Name
            };

            return View(viewModel);
        }

        // POST: GroupCrossReferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, GroupCrossReferenceViewModel groupCrossReference)
        {
            if (id != groupCrossReference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response =
                        ApiConsumer.Get<CrossReference>(
                            $"https://localhost:44305/api/CrossReference/{groupCrossReference.CrossReferenceId}");

                    if (response.Data == null)
                    {
                        return NotFound();
                    }

                    var updatedGcr = new GroupCrossReference
                    {
                        CrossReferenceId = response.Data.Id, GroupName = groupCrossReference.Name, Id = id
                    };

                    ApiConsumer.Put<GroupCrossReference>(updatedGcr, $"https://localhost:44305/api/Group/{id}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupCrossReferenceExists(groupCrossReference.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            var gcr =
                ApiConsumer.Get<GroupCrossReference>($"https://localhost:44305/api/Group/{id}/detail");

            var crossReference = gcr.Data.CrossReference;

            var serverId = crossReference.ServerId;

            var relatedCrossReferences =
                ApiConsumer.Get<List<CrossReference>>($"https://localhost:44305/api/CossReference/Server/{serverId}");

            var selectListDataSource = new List<GroupCrossReferenceViewModel>();

            foreach (var item in relatedCrossReferences.Data)
            {
                selectListDataSource.Add(new GroupCrossReferenceViewModel
                {
                    Id = item.Id,
                    CrossReferenceAlias = item.Server.Name + " - " +
                                          item.Regulation.Name + " - " +
                                          item.Branch.Name + " - " +
                                          item.Company.Name,
                    CrossReferenceId = item.Id
                });
            }

            ViewData["CrossReference"] = new SelectList(selectListDataSource, "CrossReferenceId", "CrossReferenceAlias",
                crossReference.Id);

            var viewModel = new GroupCrossReferenceViewModel
            {
                Id = gcr.Data.Id,
                Name = gcr.Data.GroupName,
                CrossReferenceAlias = gcr.Data.CrossReference.Server.Name + " - " +
                                      gcr.Data.CrossReference.Regulation.Name + " - " +
                                      gcr.Data.CrossReference.Branch.Name + " - " +
                                      gcr.Data.CrossReference.Company.Name
            };

            return View(viewModel);
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