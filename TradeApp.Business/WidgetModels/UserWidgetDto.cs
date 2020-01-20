using System;
using System.Collections.Generic;
using System.Text;

namespace TradeApp.Business.WidgetModels
{
    public class UserWidgetDto
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public List<WidgetItemDto> Item { get; set; }

    }

    public class WidgetItemDto
    {
        public int H { get; set; }
        public string I { get; set; }
        public int W { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int? WidgetId { get; set; }
        public string WidgetName { get; set; }
        public bool IsActive { get; set; }
    }

}
