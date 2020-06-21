using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Acme.Features.Orders
{
    public class ProductCatalog
    {
        private ConcurrentDictionary<string, int> productStock
            = new ConcurrentDictionary<string, int>();

        public ProductCatalog()
        {
            var products = new List<string> { "Widget", "Sprocket", "WidgetV2" };
            products.ForEach(i => productStock.AddOrUpdate(
                i, 1000, (_, __) => 1000)
            );
        }

        public bool IsInCatalog(string product) => productStock.ContainsKey(product);
        public bool TryPlaceOrder(string product, int amount)
        {
            while (true)
            {
                var stock = productStock[product];
                if (stock < amount) return false;

                if (productStock.TryUpdate(product, stock - amount, stock))
                    return true;
            }
        }

    }
}