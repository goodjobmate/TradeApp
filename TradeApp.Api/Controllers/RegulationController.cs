﻿using System;
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
    public class RegulationController : ControllerBase
    {
        private readonly BaseMetaDbContext _context;

        public RegulationController(BaseMetaDbContext context)
        {
            _context = context;
        }

        // GET: api/Regulation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Regulation>>> GetRegulations()
        {
            return await _context.Regulations.OrderBy(x => x.Id).ToListAsync();
        }

        // GET: api/Regulation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Regulation>> GetRegulation(int id)
        {
            var regulation = await _context.Regulations.FindAsync(id);

            if (regulation == null)
            {
                return NotFound();
            }

            return regulation;
        }

        // PUT: api/Regulation/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegulation(int id, Regulation regulation)
        {
            if (id != regulation.Id)
            {
                return BadRequest();
            }

            _context.Entry(regulation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegulationExists(id))
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

        // POST: api/Regulation
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Regulation>> PostRegulation(Regulation regulation)
        {
            _context.Regulations.Add(regulation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegulation", new { id = regulation.Id }, regulation);
        }

        // DELETE: api/Regulation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Regulation>> DeleteRegulation(int id)
        {
            var regulation = await _context.Regulations.FindAsync(id);
            if (regulation == null)
            {
                return NotFound();
            }

            _context.Regulations.Remove(regulation);
            await _context.SaveChangesAsync();

            return regulation;
        }

        private bool RegulationExists(int id)
        {
            return _context.Regulations.Any(e => e.Id == id);
        }
    }
}
