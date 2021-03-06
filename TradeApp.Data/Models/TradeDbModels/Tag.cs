﻿namespace TradeApp.Data.Models.TradeDbModels
{
    public class Tag : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string IncludedLogins { get; set; }
        public string ExcludedLogins { get; set; }
    }
}
