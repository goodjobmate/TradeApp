using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.UI.Controllers
{
    public class CrossReferencesController : Controller
    {
        private readonly BaseMetaDbContext _context;

        public CrossReferencesController(BaseMetaDbContext context)
        {
            _context = context;
        }

        // GET: CrossReferences
        public async Task<IActionResult> Index()
        {
            var baseMetaDbContext = _context.CrossReferences.Include(c => c.Branch)
                .Include(c => c.Company)
                .Include(c => c.Regulation).Include(c => c.Server);
            return View(await baseMetaDbContext.ToListAsync());
        }

        // GET: CrossReferences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crossReference = await _context.CrossReferences
                .Include(c => c.Branch)
                .Include(c => c.Company)
                .Include(c => c.Regulation)
                .Include(c => c.Server)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crossReference == null)
            {
                return NotFound();
            }

            return View(crossReference);
        }

        // GET: CrossReferences/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["RegulationId"] = new SelectList(_context.Regulations, "Id", "Name");
            ViewData["ServerId"] = new SelectList(_context.Servers, "Id", "Name");
            return View();
        }

        // POST: CrossReferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServerId,RegulationId,BranchId,CompanyId")]
            CrossReference crossReference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(crossReference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", crossReference.BranchId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", crossReference.CompanyId);
            ViewData["RegulationId"] = new SelectList(_context.Regulations, "Id", "Name", crossReference.RegulationId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "Id", "Name", crossReference.ServerId);
            return View(crossReference);
        }

        // GET: CrossReferences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crossReference = await _context.CrossReferences.FindAsync(id);
            if (crossReference == null)
            {
                return NotFound();
            }

            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", crossReference.BranchId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", crossReference.CompanyId);
            ViewData["RegulationId"] = new SelectList(_context.Regulations, "Id", "Name", crossReference.RegulationId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "Id", "Name", crossReference.ServerId);
            return View(crossReference);
        }

        // POST: CrossReferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServerId,RegulationId,BranchId,CompanyId")]
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
                    _context.Update(crossReference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrossReferenceExists(crossReference.Id))
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

            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", crossReference.BranchId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", crossReference.CompanyId);
            ViewData["RegulationId"] = new SelectList(_context.Regulations, "Id", "Name", crossReference.RegulationId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "Id", "Name", crossReference.ServerId);
            return View(crossReference);
        }

        // GET: CrossReferences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crossReference = await _context.CrossReferences
                .Include(c => c.Branch)
                .Include(c => c.Company)
                .Include(c => c.Regulation)
                .Include(c => c.Server)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crossReference == null)
            {
                return NotFound();
            }

            return View(crossReference);
        }

        // POST: CrossReferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crossReference = await _context.CrossReferences.FindAsync(id);
            _context.CrossReferences.Remove(crossReference);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CrossReferenceExists(int id)
        {
            return _context.CrossReferences.Any(e => e.Id == id);
        }
    }
}