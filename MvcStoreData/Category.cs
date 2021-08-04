using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public string Summary { get; set; }

        public int SortOrder { get; set; }

        public int RayonId { get; set; }

        public Rayon Rayon { get; set; }


        public ICollection<Product> Products { get; set; } = new HashSet<Product>();


    }
}