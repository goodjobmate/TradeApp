using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeApp.Business.Services.Interfaces;
using TradeApp.Business.WidgetModels;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.BaseMetaDbModels;
using TradeApp.Data.Models.TradeDbModels;

namespace TradeApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TradeDbContext _context;
        private readonly IWidgetService _widgetService;

        public TagController(IWidgetService widgetService,
            TradeDbContext context)
        {
            _widgetService = widgetService;
            _context = context;
        }

        // GET: api/Tag
        [HttpGet]
        public ActionResult<IEnumerable<ResultDto>> Get([FromQuery] int? serverId, [FromQuery] int? regulationId)
        {
            var tags = _widgetService.GetTagsWithServerAndRegulation(serverId, regulationId);

            if (!tags.Any())
            {
                return NoContent();
            }

            return Ok(tags);
        }

        // GET: api/Tag/5
        [HttpGet("{id}")]
        public ActionResult<Tag> Get(int id)
        {
            //var tag = _widgetService.GetTagById(id);
            var tag = _context.Tags.Find(id);

            if (tag == null)
            {
                return NoContent();
            }

            return Ok(tag);
        }

        // POST: api/Tag
        [HttpPost]
        public IActionResult Post([FromBody] TagDto request)
        {
            //TODO : Validations

            var (exist, id) = _widgetService.CheckIfTagExists(request);

            if (exist)
            {
                return Conflict($"There is already Tag with same filters that named {id}.");
            }

            var tag = _widgetService.CreateTag(request);

            return Ok(tag);
        }

        // PUT: api/Tag/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TagDto request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var existingTag = _context.Tags.Find(id);

            if (existingTag == null)
            {
                return NotFound();
            }

            var (exist, tagId) = _widgetService.CheckIfTagExists(request);

            if (exist)
            {
                return Conflict($"There is already Tag with same filters that named {tagId}.");
            }

            _widgetService.UpdateTag(request);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tag = _context.Tags.Find(id);

            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            _context.SaveChanges();

            return Ok();
        }


        //TODO : temp endpoints for mvc

        [HttpPut("v2/{id}")]
        public async Task<IActionResult> PutTagv2(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            _context.Entry(tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
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

        [HttpPost("v2")]
        public async Task<ActionResult<Tag>> PostTagv2(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = tag.Id }, tag);
        }

        [HttpDelete("v2/{id}")]
        public async Task<ActionResult<Tag>> DeleteTagv2(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return tag;
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }
    }
}