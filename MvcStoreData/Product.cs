using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Barcode { get; set; }

        public string Summary { get; set; }

        public int? BrandId { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public string Photo { get; set; }

        public Brand Brand { get; set; }

        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }

    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder
                .HasIndex(p => new { p.Name })
                .IsUnique();

            builder
                .HasIndex(p => new { p.Barcode })
                .IsUnique();

            builder
                .HasIndex(p => new { p.Price });

            builder
                .Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(p => p.Photo)
                .IsRequired()
                .IsUnicode(false);

            builder
                .HasMany(p => p.Comments)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(p => p.ProductImages)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(p => p.OrderItems)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}