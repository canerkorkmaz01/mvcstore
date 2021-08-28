using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Models
{
    public class SearchViewModel
    {
        internal string keywords;

        public int? CategoryId { get; set; }

        public string Keywords { get; set; }
    }
}
