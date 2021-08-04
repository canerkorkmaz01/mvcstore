using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class Comment : BaseEntity
    {

        public string Text { get; set; }
        public int Rate { get; set; }

        public ICollection<CommentLike> CommentLikes { get; set; } = new HashSet<CommentLike>();
    }
}