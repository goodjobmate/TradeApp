using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;
using TradeApp.UI.Models;

namespace TradeApp.UI.Controllers
{
    public class ServersController : Controller
    {
        private readonly BaseMetaDbContext _context;

        public ServersController(BaseMetaDbContext context)
        {
            _context = context;
        }

        // GET: Servers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Servers.ToListAsync());
        }

        // GET: Servers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = from cr in _context.CrossReferences
                where cr.ServerId == id
                join groupCrossReference in _context.GroupCrossReferences on cr.Id equals groupCrossReference
                    .CrossReferenceId
                join server in _context.Servers on cr.ServerId equals server.Id
                join regulation in _context.Regulations on cr.RegulationId equals regulation.Id
                join branch in _context.Branches on cr.BranchId equals branch.Id
                join company in _context.Companies on cr.CompanyId equals company.Id
                select new GroupCrossReferenceViewModel
                {
                    ServerName = server.Name,
                    RegulationName = regulation.Name,
                    BranchName = branch.Name,
                    CompanyName = company.Name,
                    GroupName = groupCrossReference.GroupName,
                    ServerId = server.Id,
                    RegulationId = regulation.Id,
                    BranchId = branch.Id,
                    CompanyId = company.Id,
                    CrossReferenceId = cr.Id,
                    GroupCrossReferenceId = groupCrossReference.Id
                };

            return View(await response.ToListAsync());
        }

        // GET: Servers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Servers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Region,Dns,IpAddress,MetaType,MetaVersion,Description")] Server server)
        {
            if (ModelState.IsValid)
            {
                _context.Add(server);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(server);
        }

        // GET: Servers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var server = await _context.Servers.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }
            return View(server);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Region,Dns,IpAddress,MetaType,MetaVersion,Description")] Server server)
        {
            if (id != server.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(server);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServerExists(server.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(server);
        }

        // GET: Servers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var server = await _context.Servers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (server == null)
            {
                return NotFound();
            }

            return View(server);
        }

        // POST: Servers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var server = await _context.Servers.FindAsync(id);
            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServerExists(int id)
        {
            return _context.Servers.Any(e => e.Id == id);
        }

        public IActionResult EditGroup()
        {
            throw new System.NotImplementedException();
        }
    }
}
