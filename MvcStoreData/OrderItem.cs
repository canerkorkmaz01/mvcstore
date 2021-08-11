using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcStoreData
{

    public class OrderItem : BaseEntity
    {
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [NotMapped]
        public decimal LineTotal => Price * Quantity;

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }

    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder
                .Property(p => p.Price)
                .HasPrecision(18, 4);

        }
    }
}