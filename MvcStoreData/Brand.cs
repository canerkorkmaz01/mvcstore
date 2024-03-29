﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcStoreData
{
    public class Brand : SortableBaseEntity
    {
        [Display(Name = "Marka Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Name { get; set; }

        public string Photo { get; set; }

        [NotMapped]
        [Display(Name = "Logo")]
        public IFormFile PhotoFile { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

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