using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TradeApp.Business.Services.Interfaces;
using TradeApp.Business.WidgetModels;

namespace TradeApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetController : ControllerBase
    {
        private const int UserId = 1;

        private readonly IWidgetService _widgetService;

        public WidgetController(IWidgetService widgetService)
        {
            _widgetService = widgetService;
        }

        // GET: api/Widget
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
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

        [HttpGet("LookUp")]
        public ActionResult<List<ResultDto>> GetLookUp()
        {
            return Ok(_widgetService.GetLookUp());
        }

        [HttpGet("Menu/{userId:int}")]
        public ActionResult<List<ResultDto>> GetMenu(int userId)
        {
            return Ok(_widgetService.GetMenu(userId));
        }

        // POST: api/Widget
        [HttpPost]
        public ActionResult<int> Post([FromBody] UserDashboardWidgetDto userDashboardWidget)
        {
            //TODO : Validations

            return Ok(_widgetService.CreateUserDashboardWidget(UserId, userDashboardWidget));
        }

        // PUT: api/Widget/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //TODO : post - put - patch ayrilmali
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var widget = _widgetService.GetWidget(id);

            if (widget == null)
            {
                return NotFound();
            }

            _widgetService.DeleteWidget(widget.PageId);
            return Ok();
        }


        #region Filtering Operation

        
        // POST: api/Widget/Filter
        [HttpPost("Filter")]
        public ActionResult<int> PostFilter([FromBody] WidgetFilterDto widgetFilterDto)
        {
            return Ok(_widgetService.CreateUserDashboardWidgetFilter(UserId, widgetFilterDto));
        }

       
        // GET: api/Widget/Filter
        [HttpGet("Filter")]
        public ActionResult<int> GetFilter(int userDashboardWidgetId)
        {
            return Ok(_widgetService.GetUserDashboardWidgetFilter(userDashboardWidgetId));
        }

        // GET: api/Widget/ExposureFilter
        [HttpGet("ExposureFilter")]
        public ActionResult<int> GetExposureFilter(int userDashboardWidgetId)
        {
            return Ok(_widgetService.GetExposureUserDashboardWidgetFilter(userDashboardWidgetId));
        }

        #endregion
    }
}