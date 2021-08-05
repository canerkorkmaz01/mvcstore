using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class Rayon : SortableBaseEntity
    {
        public string Name { get; set; }

        public string Summary { get; set; }

        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();

    }
    public class RayonEntityTypeConfiguration : IEntityTypeConfiguration<Rayon>
    {
        public void Configure(EntityTypeBuilder<Rayon> builder)
        {

            builder
                .HasIndex(p => new { p.Name })
                .IsUnique();

            builder
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();


            builder
                .HasMany(p => p.Categories)
                .WithOne(p => p.Rayon)
                .HasForeignKey(p => p.RayonId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}