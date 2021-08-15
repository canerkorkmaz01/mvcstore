using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcStoreData
{
    public class Banner : SortableBaseEntity
    {
        public string Name { get; set; }

        public string Image { get; set; }

        [Display(Name = "İlk T.")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateFirst { get; set; }

        [Display(Name = "Son T.")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateLast { get; set; }

        public string Url { get; set; }

        public bool IsDefault { get; set; }

        [NotMapped]
        [Display(Name = "Logo")]
        public IFormFile PhotoFile { get; set; }

    }

    public class BannerEntityTypeConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder
                .Property(p => p.Image)
                .IsRequired()
                .IsUnicode(false);
            
        }
    }

}