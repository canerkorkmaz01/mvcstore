﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MvcStoreData
{
    public enum OrderStatus
    {
        New, Shipped, Cancelled
    }

    public class Order : BaseEntity
    {
        public OrderStatus OrderStatus { get; set; }

        public string CargoCode { get; set; }

        [NotMapped]
        public decimal GrandTotal => OrderItems.Sum(p => p.LineTotal);

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }

    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder
                .HasMany(p => p.OrderItems)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}