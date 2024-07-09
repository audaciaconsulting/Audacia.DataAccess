using System;
using System.Collections.Generic;

namespace Audacia.DataAccess.Tests.Helpers.Entities;

public class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public DateTime DatePlaced { get; set; }
}