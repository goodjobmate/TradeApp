﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;
using TradeApp.UI.Options;

namespace TradeApp.UI.Controllers
{
    public class ServerController : Controller
    {
        private readonly IOptions<ApiOptions> _options;

        public ServerController(IOptions<ApiOptions> options)
        {
            _options = options;
        }

        // GET: Server
        public IActionResult Index()
        {
            var response = ApiConsumer.Get<List<Server>>(_options.Value.ApiUrl + "server");
            return View(response.Data);
        }

        // GET: Server/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Server>(_options.Value.ApiUrl + $"server/{id}");
            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // GET: Server/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Server/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Region,Dns,IpAddress,MetaType,MetaVersion,Description")]
            Server server)
        {
            if (ModelState.IsValid)
            {
                ApiConsumer.Post<Server>(server, _options.Value.ApiUrl + "server");
                return RedirectToAction(nameof(Index));
            }

            return View(server);
        }

        // GET: Server/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Server>(_options.Value.ApiUrl + $"server/{id}");
            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: Server/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,
            [Bind("Id,Name,Region,Dns,IpAddress,MetaType,MetaVersion,Description")]
            Server server)
        {
            if (id != server.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ApiConsumer.Put<Server>(server, _options.Value.ApiUrl + $"server/{id}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServerExists(server.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(server);
        }

        // GET: Server/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = ApiConsumer.Get<Server>(_options.Value.ApiUrl + $"server/{id}");
            if (response.Data == null)
            {
                return NotFound();
            }

            return View(response.Data);
        }

        // POST: Server/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ApiConsumer.Delete<Server>(_options.Value.ApiUrl + $"server/{id}");
            return RedirectToAction(nameof(Index));
        }

        private bool ServerExists(int id)
        {
            return ApiConsumer.Get<Server>(_options.Value.ApiUrl + $"Server/{id}").Data != null;
        }
    }
}