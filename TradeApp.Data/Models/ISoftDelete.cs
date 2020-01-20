namespace TradeApp.Data.Models
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}