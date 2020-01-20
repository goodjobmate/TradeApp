using System.ComponentModel.DataAnnotations.Schema;

namespace TradeApp.Data.Models.TradeDbModels
{
    public class UserDashboardWidgetFilter : BaseEntity<int>
    {
        public int UserDashboardWidgetId { get; set; }
        public int FilterId { get; set; }
        public string Name { get; set; }


        [ForeignKey("FilterId")]
        public Filter Filter { get; set; }

        [ForeignKey("UserDashboardWidgetId")]
        public UserDashboardWidget UserDashboardWidget { get; set; }
    }
}