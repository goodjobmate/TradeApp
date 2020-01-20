using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeApp.Business.Services.Interfaces;
using TradeApp.Business.WidgetModels;

namespace TradeApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetController : ControllerBase
    {
        private readonly IWidgetService _widgetService;
        public WidgetController(IWidgetService widgetService)
        {
            _widgetService = widgetService;
        }

        // GET: api/Widget
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Widget/5
        [HttpGet("{id}")]
        public ActionResult<UserWidgetDto> Get(int id)
        {
            var widget = _widgetService.GetWidget(id);

            if (widget == null)
            {
                return NoContent();
            }

            return Ok(widget);
        }

        // POST: api/Widget
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Widget/5
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
