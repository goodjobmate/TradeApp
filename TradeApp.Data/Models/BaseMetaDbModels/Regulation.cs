using System.ComponentModel.DataAnnotations.Schema;

namespace TradeApp.Data.Models.BaseMetaDbModels
{
    [Table("regulations")]
    public class Regulation : IEntity<int>
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
