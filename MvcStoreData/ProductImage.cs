using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class ProductImage : SortableBaseEntity
    {

        public string Photo { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

    }

    public class ProductImageEntityTypeConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {


            builder
                .Property(p => p.Photo)
                .IsUnicode(false);

        }
    }
}