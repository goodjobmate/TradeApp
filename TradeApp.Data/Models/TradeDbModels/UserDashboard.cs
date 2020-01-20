using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeApp.Data.Models.TradeDbModels
{
    public class UserDashboard : BaseEntity<int>
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<UserDashboardWidget> UserDashboardWidgets { get; set; }



        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}