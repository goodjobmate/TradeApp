using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TradeApp.Business.Services.Interfaces;
using TradeApp.Business.WidgetModels;

namespace TradeApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IWidgetService _widgetService;

        public TagController(IWidgetService widgetService)
        {
            _widgetService = widgetService;
        }

        // GET: api/Tag
        [HttpGet]
        public ActionResult<List<ResultDto>> Get([FromQuery] int? serverId, [FromQuery] int? regulationId)
        {
            var tags = _widgetService.GetTagsWithServerAndRegulation(serverId, regulationId);

            if (!tags.Any())
            {
                return NoContent();
            }

            return Ok(tags);
        }

        // GET: api/Tag/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Tag
        [HttpPost]
        public IActionResult Post([FromBody] AddTagRequest request)
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
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}