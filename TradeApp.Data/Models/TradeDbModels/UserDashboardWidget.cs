using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeApp.Data.Models.TradeDbModels
{
    public class UserDashboardWidget : BaseEntity<int>
    {
        public int? WidgetId { get; set; }
        public int UserDashboardId { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int XAxis { get; set; }
        public int YAxis { get; set; }
        public string Index { get; set; }
        public List<UserDashboardWidgetFilter> Filters { get; set; }



        [ForeignKey("WidgetId")]
        public Widget Widget { get; set; }

        [ForeignKey("UserDashboardId")]
        public UserDashboard UserDashboard { get; set; }
    }
}