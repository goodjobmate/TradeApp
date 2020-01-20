using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;

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
        public async Task<IActionResult> Create([Bind("Id,CrossReferenceId,GroupName")] GroupCrossReference groupCrossReference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupCrossReference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CrossReferenceId"] = new SelectList(_context.CrossReferences, "Id", "Id", groupCrossReference.CrossReferenceId);
            return View(groupCrossReference);
        }

        // GET: GroupCrossReferences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupCrossReference = await _context.GroupCrossReferences.FindAsync(id);
            if (groupCrossReference == null)
            {
                return NotFound();
            }
            ViewData["CrossReferenceId"] = new SelectList(_context.CrossReferences, "Id", "Id", groupCrossReference.CrossReferenceId);
            return View(groupCrossReference);
        }

        // POST: GroupCrossReferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CrossReferenceId,GroupName")] GroupCrossReference groupCrossReference)
        {
            if (id != groupCrossReference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupCrossReference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupCrossReferenceExists(groupCrossReference.Id))
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
            ViewData["CrossReferenceId"] = new SelectList(_context.CrossReferences, "Id", "Id", groupCrossReference.CrossReferenceId);
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
        [HttpPost, ActionName("Delete")]
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
