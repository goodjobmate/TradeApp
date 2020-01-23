using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeApp.Business.BaseMetaModels;
using TradeApp.Business.Services.Interfaces;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;

namespace TradeApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly BaseMetaDbContext _context;
        private readonly IBaseMetaService _baseMetaService;

        public GroupController(BaseMetaDbContext context,
            IBaseMetaService baseMetaService)
        {
            _context = context;
            _baseMetaService = baseMetaService;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupCrossReference>>> GetGroupCrossReferences()
        {
            return await _context.GroupCrossReferences.OrderBy(x => x.Id).ToListAsync();
        }

        [HttpGet("details")]
        public async Task<ActionResult<IEnumerable<GroupCrossReference>>> GetGroupCrossReferencesWithDetails()
        {
            return await _context.GroupCrossReferences
                .Include(x => x.CrossReference).ThenInclude(x => x.Server)
                .Include(x => x.CrossReference).ThenInclude(x => x.Regulation)
                .Include(x => x.CrossReference).ThenInclude(x => x.Branch)
                .Include(x => x.CrossReference).ThenInclude(x => x.Company)
                .OrderBy(x => x.Id).ToListAsync();
        }

        [HttpGet("Server/{serverId:int}")]
        public ActionResult<IEnumerable<GroupCrossReferenceResponse>> GetGroupsByServer(int serverId)
        {
            var response = _baseMetaService.GetGroupsByServerId(serverId);

            if (!response.Any())
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}/detail")]
        public ActionResult<GroupCrossReference> GetGroupDetail(int id)
        {
            var response = _context.GroupCrossReferences
                .Include(x => x.CrossReference).ThenInclude(x => x.Server)
                .Include(x => x.CrossReference).ThenInclude(x => x.Regulation)
                .Include(x => x.CrossReference).ThenInclude(x => x.Branch)
                .Include(x => x.CrossReference).ThenInclude(x => x.Company).FirstOrDefault(x => x.Id == id);

            if (response == null)
            {
                return NotFound();
            }

            return response;
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
