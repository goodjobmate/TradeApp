using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;
using TradeApp.UI.Models;

namespace TradeApp.UI.Controllers
{
    public class GroupCrossReferencesController : Controller
    {
        private readonly BaseMetaDbContext _context;

        public GroupCrossReferencesController(BaseMetaDbContext context)
        {
            _context = context;
        }

        // GET: GroupCrossReferences
        public async Task<IActionResult> Index()
        {
            var baseMetaDbContext = _context.GroupCrossReferences.Include(g => g.CrossReference);
            return View(await baseMetaDbContext.ToListAsync());
        }

        // GET: GroupCrossReferences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupCrossReference = await _context.GroupCrossReferences
                .Include(g => g.CrossReference)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupCrossReference == null)
            {
                return NotFound();
            }

            return View(groupCrossReference);
        }

        // GET: GroupCrossReferences/Create
        public IActionResult Create()
        {
            ViewData["CrossReferenceId"] = new SelectList(_context.CrossReferences, "Id", "Id");
            return View();
        }

        // POST: GroupCrossReferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CrossReferenceId,GroupName")]
            GroupCrossReference groupCrossReference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupCrossReference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CrossReferenceId"] =
                new SelectList(_context.CrossReferences, "Id", "Id", groupCrossReference.CrossReferenceId);
            return View(groupCrossReference);
        }

        // GET: GroupCrossReferences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = from cr in _context.CrossReferences
                join gcr in _context.GroupCrossReferences on cr.Id equals gcr
                    .CrossReferenceId
                where gcr.Id == id
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
                    GroupName = gcr.GroupName,
                    ServerId = server.Id,
                    RegulationId = regulation.Id,
                    BranchId = branch.Id,
                    CompanyId = company.Id,
                    CrossReferenceId = cr.Id,
                    GroupCrossReferenceId = gcr.Id
                };

            var viewModel = await query.FirstOrDefaultAsync();

            ViewData["ServerName"] = new SelectList(_context.Servers, "Id", "Name", viewModel.ServerId);
            ViewData["RegulationName"] = new SelectList(_context.Regulations, "Id", "Name", viewModel.RegulationId);
            ViewData["BranchName"] = new SelectList(_context.Branches, "Id", "Name", viewModel.BranchId);
            ViewData["CompanyName"] = new SelectList(_context.Companies, "Id", "Name", viewModel.CompanyId);
            return View(viewModel);
        }

        // POST: GroupCrossReferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GroupCrossReferenceViewModel groupCrossReference)
        {
            if (id != groupCrossReference.GroupCrossReferenceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var crossReference = await _context.CrossReferences.FirstOrDefaultAsync(x =>
                        x.ServerId == groupCrossReference.ServerId
                        && x.RegulationId == groupCrossReference.RegulationId
                        && x.BranchId == groupCrossReference.BranchId
                        && x.CompanyId == groupCrossReference.CompanyId);

                    if (crossReference == null)
                    {
                        return NotFound();
                    }

                    var gcr = await _context.GroupCrossReferences.FindAsync(groupCrossReference.GroupCrossReferenceId);
                    gcr.CrossReferenceId = crossReference.Id;
                    gcr.GroupName = groupCrossReference.GroupName;

                    _context.Update(gcr);

                    await _context.SaveChangesAsync();
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

            ViewData["ServerName"] = new SelectList(_context.Servers, "Id", "Name", groupCrossReference.ServerId);
            ViewData["RegulationName"] =
                new SelectList(_context.Regulations, "Id", "Name", groupCrossReference.RegulationId);
            ViewData["BranchName"] = new SelectList(_context.Branches, "Id", "Name", groupCrossReference.BranchId);
            ViewData["CompanyName"] = new SelectList(_context.Companies, "Id", "Name", groupCrossReference.CompanyId);
            return View(groupCrossReference);
        }

        // GET: GroupCrossReferences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupCrossReference = await _context.GroupCrossReferences
                .Include(g => g.CrossReference)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupCrossReference == null)
            {
                return NotFound();
            }

            return View(groupCrossReference);
        }

        // POST: GroupCrossReferences/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupCrossReference = await _context.GroupCrossReferences.FindAsync(id);
            _context.GroupCrossReferences.Remove(groupCrossReference);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupCrossReferenceExists(int id)
        {
            return _context.GroupCrossReferences.Any(e => e.Id == id);
        }
    }
}