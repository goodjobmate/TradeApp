using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrossReferenceController : ControllerBase
    {
        private readonly BaseMetaDbContext _context;

        public CrossReferenceController(BaseMetaDbContext context)
        {
            _context = context;
        }

        // GET: api/CrossReference
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrossReference>>> GetCrossReferences()
        {
            return await _context.CrossReferences.ToListAsync();
        }

        // GET: api/CrossReference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CrossReference>> GetCrossReference(int id)
        {
            var crossReference = await _context.CrossReferences.FindAsync(id);

            if (crossReference == null)
            {
                return NotFound();
            }

            return crossReference;
        }

        [HttpGet("{id}/detail")]
        public async Task<ActionResult<CrossReference>> GetCrossReferenceDetail(int id)
        {
            var crossReference = await _context.CrossReferences
                .Include(x => x.Server)
                .Include(x => x.Regulation)
                .Include(x => x.Branch)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (crossReference == null)
            {
                return NotFound();
            }

            return crossReference;
        }

        // PUT: api/CrossReference/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCrossReference(int id, CrossReference crossReference)
        {
            if (id != crossReference.Id)
            {
                return BadRequest();
            }

            _context.Entry(crossReference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CrossReferenceExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/CrossReference
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CrossReference>> PostCrossReference(CrossReference crossReference)
        {
            _context.CrossReferences.Add(crossReference);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCrossReference", new {id = crossReference.Id}, crossReference);
        }

        // DELETE: api/CrossReference/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CrossReference>> DeleteCrossReference(int id)
        {
            var crossReference = await _context.CrossReferences.FindAsync(id);
            if (crossReference == null)
            {
                return NotFound();
            }

            _context.CrossReferences.Remove(crossReference);
            await _context.SaveChangesAsync();

            return crossReference;
        }

        private bool CrossReferenceExists(int id)
        {
            return _context.CrossReferences.Any(e => e.Id == id);
        }
    }
}