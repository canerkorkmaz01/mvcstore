using MvcStoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Sys
{
    public class ShoppingCart
    {
        public IList<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public decimal GrandTotal => Items.Sum(p => p.Amount);

        public void Add(Product product, int quantity)
        {
            var item = Items.SingleOrDefault(p => p.Id == product.Id);
            if (item != null)
                item.Quantity += quantity;
            else
            {
                Items.Add(new ShoppingCartItem
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.DiscountedPrice,
                    Quantity = quantity
                });
            }
        }

        public void Remove(int id)
        {
            var item = Items.Single(p => p.Id == id);
            Items.Remove(item);
        }

        public void Clear()
        {
            Items.Clear();
        }

    }
}
