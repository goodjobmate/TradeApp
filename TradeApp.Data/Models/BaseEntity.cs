using System;

namespace TradeApp.Data.Models
{
    public class BaseEntity<T> : IEntity<T>, ISoftDelete where T : IComparable
    {
        public T Id { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}