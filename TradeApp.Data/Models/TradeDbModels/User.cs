using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TradeApp.Data.Models.TradeDbModels
{
    public class User : BaseEntity<int>
    {
        [Key]
        public int UserId { get; set; }

        public List<UserDashboard> UserDashboards { get; set; }
    }
}
