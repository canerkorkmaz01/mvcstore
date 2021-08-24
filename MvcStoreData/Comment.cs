using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcStoreData
{
    public class Comment : BaseEntity
    {
        [Display(Name ="Yorumunuz")]
        [Required(ErrorMessage = "Yorum boş bırakılamaz!")]
        public string Text { get; set; }

        [Display(Name ="Puan")]
        public int Rate { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<CommentLike> CommentLikes { get; set; } = new HashSet<CommentLike>();
    }


    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {

            builder
                .Property(p => p.Text)
                .IsRequired();

            builder
                .HasMany(p => p.CommentLikes)
                .WithOne(p => p.Comment)
                .HasForeignKey(p => p.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}