using System.Collections.Generic;
using TradeApp.Data.Models.TradeDbModels;

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

    public class TagDto
    {
        public TagDto()
        {
            TagFilter = new TagFilterDto();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public TagFilterDto TagFilter { get; set; }
    }

    public class TagFilterDto
    {
        public TagFilterDto()
        {
            ServerIds = new List<int>();
            Groups = new Dictionary<int, List<string>>();
            XIds = new List<int>();
            IncludedLogins = new Dictionary<int, List<int>>();
            ExcludedLogins = new Dictionary<int, List<int>>();
            TagIds = new List<int>();
            OperatorByCalculation = new Dictionary<WidgetType, bool>();
        }

        public int? RegulationId { get; set; }
        public List<int> ServerIds { get; set; }
        public Dictionary<int, List<string>> Groups { get; set; }
        public List<int> XIds { get; set; }
        public Dictionary<int, List<int>> IncludedLogins { get; set; }
        public Dictionary<int, List<int>> ExcludedLogins { get; set; }
        public List<int> TagIds { get; set; }
        public Dictionary<WidgetType, bool> OperatorByCalculation { get; set; }
    }
}