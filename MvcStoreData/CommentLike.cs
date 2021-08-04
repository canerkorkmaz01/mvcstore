using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class CommentLike: BaseEntity
    {
        public int CommentId { get; set; }

        public bool Like { get; set; }

        public Comment Comment { get; set; }
    }
}