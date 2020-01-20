using System.ComponentModel.DataAnnotations.Schema;

namespace TradeApp.Data.Models.BaseMetaDbModels
{
    [Table("servers")]
    public class Server : IEntity<int>
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("region")]
        public string Region { get; set; }
        [Column("dns")]
        public string Dns { get; set; }
        [Column("ipaddress")]
        public string IpAddress { get; set; }
        [Column("metatype")]
        public short MetaType { get; set; }
        [Column("metaversion")]
        public  bool? MetaVersion { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
