using System.Collections.Generic;

namespace Audacia.DataAccess.Tests.Helpers.Entities;

public class Product
{
    public int ProductId { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}