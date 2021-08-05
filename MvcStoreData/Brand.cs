using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class Brand : SortableBaseEntity
    {
        public string Name { get; set; }

        public string Photo { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();

    }

    public class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {

            builder
                .HasIndex(p => new { p.Name })
                .IsUnique();

            builder
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(p => p.Photo)
                .IsRequired()
                .IsUnicode(false);

            builder
                .HasMany(p => p.Products)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }

}