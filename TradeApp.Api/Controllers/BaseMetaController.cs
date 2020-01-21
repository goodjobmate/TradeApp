using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TradeApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseMetaController : ControllerBase
    {
        //TODO : muhammet.akdemir
        // GET: api/BaseMeta
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
        }

        // GET: api/BaseMeta/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok("value");
        }

        // POST: api/BaseMeta
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/BaseMeta/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/BaseMeta/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}