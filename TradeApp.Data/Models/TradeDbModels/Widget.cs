namespace TradeApp.Data.Models.TradeDbModels
{
    public class Widget : BaseEntity<int>
    {
        public string Name { get; set; }
        public WidgetType Type { get; set; }
    }
}