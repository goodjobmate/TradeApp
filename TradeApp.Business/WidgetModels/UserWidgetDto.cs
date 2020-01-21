using System.Collections.Generic;

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

    public class ResultDto
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }

    public class UserDashboardWidgetDto
    {
        public int? PageId { get; set; }
        public string PageName { get; set; }
        public ItemDto Item { get; set; }
        public List<WidgetDto> Widget { get; set; }
    }

    public class ItemDto
    {
        public List<LgDto> Lg { get; set; }
    }

    public class WidgetDto
    {
        public int IndexId { get; set; }
        public string Name { get; set; }
    }

    public class LgDto
    {
        public int W { get; set; }
        public int H { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string I { get; set; }
        public bool Moved { get; set; }
    }
}