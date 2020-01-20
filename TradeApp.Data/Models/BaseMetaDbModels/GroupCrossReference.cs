using System.ComponentModel.DataAnnotations.Schema;

namespace TradeApp.Data.Models.BaseMetaDbModels
{
    [Table("groupcrossreferences")]
    public class GroupCrossReference : IEntity<int>
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("xid")]
        public int CrossReferenceId { get; set; }
        [Column("group_name")]
        public string GroupName { get; set; }

        public CrossReference CrossReference { get; set; }
    }
}
