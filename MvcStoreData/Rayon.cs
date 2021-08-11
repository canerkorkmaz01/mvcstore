using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcStoreData
{
    public class Rayon : SortableBaseEntity
    {
        [Display(Name = "Reyon Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Name { get; set; }

        [Display(Name = "Açıklamalar")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();

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