using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class Brand : SortableBaseEntity
    {
        public string Name { get; set; }

        public string Photo { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();

    }
}