using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class Rayon : SortableBaseEntity
    {
        public string Name { get; set; }

        public string Summary { get; set; }

        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();

    }
}