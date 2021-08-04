using MvcStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcStoreData
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Summary { get; set; }

        public int? BrandId { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public string Photo { get; set; }

        public Brand Brand { get; set; }

        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    }
}