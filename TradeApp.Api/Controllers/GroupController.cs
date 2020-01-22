using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly BaseMetaDbContext _context;

        public GroupController(BaseMetaDbContext context)
        {
            _context = context;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupCrossReference>>> GetGroupCrossReferences()
        {
            return await _context.GroupCrossReferences.ToListAsync();
        }

        // GET: api/Group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupCrossReference>> GetGroupCrossReference(int id)
        {
            var groupCrossReference = await _context.GroupCrossReferences.FindAsync(id);

            if (groupCrossReference == null)
            {
                return NotFound();
            }

            return groupCrossReference;
        }

        // PUT: api/Group/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupCrossReference(int id, GroupCrossReference groupCrossReference)
        {
            if (id != groupCrossReference.Id)
            {
                return BadRequest();
            }

            _context.Entry(groupCrossReference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupCrossReferenceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Group
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<GroupCrossReference>> PostGroupCrossReference(GroupCrossReference groupCrossReference)
        {
            _context.GroupCrossReferences.Add(groupCrossReference);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupCrossReference", new { id = groupCrossReference.Id }, groupCrossReference);
        }

        // DELETE: api/Group/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupCrossReference>> DeleteGroupCrossReference(int id)
        {
            var groupCrossReference = await _context.GroupCrossReferences.FindAsync(id);
            if (groupCrossReference == null)
            {
                return NotFound();
            }

            _context.GroupCrossReferences.Remove(groupCrossReference);
            await _context.SaveChangesAsync();

            return groupCrossReference;
        }

        private bool GroupCrossReferenceExists(int id)
        {
            return _context.GroupCrossReferences.Any(e => e.Id == id);
        }
    }
}
