using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcStoreData
{
    public class Product : BaseEntity
    {
        [Display(Name = "Ürün Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Name { get; set; }

        [Display(Name = "Ürün Kodu")]
        public string Code { get; set; }

        [Display(Name = "Barkod")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Lütfen geçerli bir barkod yazınız!")]
        public string Barcode { get; set; }

        [Display(Name = "Açıklamalar")]
        public string Summary { get; set; }

        [Display(Name = "Marka")]
        public int? BrandId { get; set; }

        public decimal Price { get; set; }

        [NotMapped]
        [Display(Name = "Fiyat")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        [RegularExpression(@"^\d+(,\d{1,2})?$", ErrorMessage = "Lütfen geçerli bir fiyat yazınız!")]
        public string PriceText { get; set; }


        [Display(Name = "İndirim Oranı (%)")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        [Range(0, 99, ErrorMessage = "Lütfen 0 ile 99 arası bir indirim oranı yazınız!")]
        public int Discount { get; set; }

        public string Photo { get; set; }


        [NotMapped]
        [Display(Name = "Foto")]
        public IFormFile PhotoFile { get; set; }

        [NotMapped]
        public int[] SelectedCategories { get; set; }
        
        [NotMapped]
        public int[] PicturesToDeleted { get; set; }

        [NotMapped]
        [Display(Name = "Foto Galeri")]
        public IFormFile[] PhotoFiles { get; set; }

        [NotMapped]
        public decimal DiscountedPrice => Price - (Price * Discount / 100m);

        public virtual Brand Brand { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
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
                .Property(p => p.Price)
                .HasPrecision(18, 4);

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