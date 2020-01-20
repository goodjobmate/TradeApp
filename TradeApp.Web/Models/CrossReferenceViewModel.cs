namespace TradeApp.Web.Models
{
    public class CrossReferenceViewModel
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public int RegulationId { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public string ServerName { get; set; }
        public string RegulationName { get; set; }
        public string BranchName { get; set; }
        public string CompanyName { get; set; }
    }
}
