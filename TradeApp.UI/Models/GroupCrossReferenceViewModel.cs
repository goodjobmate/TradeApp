using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeApp.UI.Models
{
    public class GroupCrossReferenceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CrossReferenceAlias { get; set; }
        public int CrossReferenceId { get; set; }
    }
}
