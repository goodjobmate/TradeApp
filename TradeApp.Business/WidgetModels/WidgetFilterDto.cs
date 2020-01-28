using System.Collections.Generic;

namespace TradeApp.Business.WidgetModels
{
    public class WidgetFilterDto
    {
        public int UserDashboardWidgetId { get; set; }
        public List<FilterSetDto> Filters { get; set; }
    }

    public class FilterSetDto
    {
        public List<FilterDto> FilterSet { get; set; }
        public string Name { get; set; }
    }

    public class FilterDto
    {
        public int? RegulationId { get; set; }
        public List<int> ServerIds { get; set; }
        public List<int> XIds { get; set; }
        public List<string> Groups { get; set; }
        public List<int> IncludedTagIds { get; set; }
        public List<int> ExcludedTagIds { get; set; }
        public int? DataCount { get; set; }
    }
}
