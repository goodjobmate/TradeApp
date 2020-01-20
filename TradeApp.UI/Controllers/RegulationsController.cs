using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.UI.Controllers
{
    public class RegulationsController : Controller
    {
        private readonly BaseMetaDbContext _context;

        public RegulationsController(BaseMetaDbContext context)
        {
            _context = context;
        }

        // GET: Regulations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Regulations.ToListAsync());
        }

        // GET: Regulations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regulation = await _context.Regulations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (regulation == null)
            {
                return NotFound();
            }

            return View(regulation);
        }

        // GET: Regulations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Regulations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Regulation regulation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(regulation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(regulation);
        }

        // GET: Regulations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regulation = await _context.Regulations.FindAsync(id);
            if (regulation == null)
            {
                return NotFound();
            }
            return View(regulation);
        }

        // POST: Regulations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Regulation regulation)
        {
            if (id != regulation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(regulation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegulationExists(regulation.Id))
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
            return View(regulation);
        }

        // GET: Regulations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regulation = await _context.Regulations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (regulation == null)
            {
                return NotFound();
            }

            return View(regulation);
        }

        // POST: Regulations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var regulation = await _context.Regulations.FindAsync(id);
            _context.Regulations.Remove(regulation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegulationExists(int id)
        {
            return _context.Regulations.Any(e => e.Id == id);
        }
    }
}
