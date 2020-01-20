namespace TradeApp.Data.Models
{
    public class Tag : IEntity<int>

    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Logins { get; set; }
        public int Id { get; set; }
    }
}
