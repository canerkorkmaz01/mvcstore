using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Models
{
    public class LikeResultModel
    {
        public bool Error { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

    }
}
