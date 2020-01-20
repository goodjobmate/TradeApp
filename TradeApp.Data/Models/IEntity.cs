using System;

namespace TradeApp.Data.Models
{
    public interface IEntity<T> where T : IComparable
    {
        T Id { get; set; }
    }
}