using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class CommentLike : BaseEntity
    {
        public int CommentId { get; set; }

        public bool Like { get; set; }

        public Comment Comment { get; set; }
    }

    public class CommentLikeEntityTypeConfiguration : IEntityTypeConfiguration<CommentLike>
    {
        public void Configure(EntityTypeBuilder<CommentLike> builder)
        {

        }
    }
}