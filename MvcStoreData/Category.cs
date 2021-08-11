using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcStoreData
{
    public class Category : SortableBaseEntity
    {
        [Display(Name = "Kategori Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Name { get; set; }

        [Display(Name = "Açıklamalar")]
        public string Summary { get; set; }

        [Display(Name = "Reyon")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public int RayonId { get; set; }

        public virtual Rayon Rayon { get; set; }


        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();


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