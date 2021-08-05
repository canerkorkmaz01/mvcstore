using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class Category : SortableBaseEntity
    {
        public string Name { get; set; }

        public string Summary { get; set; }

        public int RayonId { get; set; }

        public Rayon Rayon { get; set; }


        public ICollection<Product> Products { get; set; } = new HashSet<Product>();


    }


    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder
                .HasIndex(p => new { p.Name, p.RayonId })
                .IsUnique();

            builder
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

        }
    }
}