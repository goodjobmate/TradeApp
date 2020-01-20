using System.ComponentModel.DataAnnotations.Schema;

namespace TradeApp.Data.Models.BaseMetaDbModels
{
    [Table("crossreferences")]
    public class CrossReference : IEntity<int>
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("serverid")]
        public int ServerId { get; set; }
        [Column("regulationid")]
        public int RegulationId { get; set; }
        [Column("branchid")]
        public int BranchId { get; set; }
        [Column("companyid")]
        public int CompanyId { get; set; }

        public Server Server { get; set; }
        public Regulation Regulation { get; set; }
        public Branch Branch { get; set; }
        public Company Company { get; set; }
    }
}
