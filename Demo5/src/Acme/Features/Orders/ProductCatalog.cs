using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prometheus;

namespace Acme.Features.Orders
{
    public class ProductCatalog
    {
        private ConcurrentDictionary<string, int> productStock
            = new ConcurrentDictionary<string, int>();

        private static readonly Gauge StockGauge =
            Metrics.CreateGauge("product_stock_levels", "Product inventory levels", "product");

        public ProductCatalog()
        {
            var products = new List<string> { "Widget", "Sprocket", "WidgetV2" };
            products.ForEach(i => Restock(i, 1000));
        }

        public bool IsInCatalog(string product) => productStock.ContainsKey(product);
        public bool TryPlaceOrder(string product, int amount)
        {
            while (true)
            {
                var stock = productStock[product];
                if (stock < amount) return false;

                var newStock = stock - amount;


                if (productStock.TryUpdate(product, newStock, stock))
                {
                    StockGauge.WithLabels(product).Dec(amount);
                    return true;
                }
            }
        }

        public void Restock(string product, int amount)
        {
            int AddStock(string product)
            {
                StockGauge.WithLabels(product).Set(amount);
                return amount;
            }

            int UpdateStock(string product, int existing)
            {
                StockGauge.WithLabels(product).Inc(amount - existing);
                return amount;
            }

            productStock.AddOrUpdate(product, AddStock, UpdateStock);
        }

        public string[] GetProducts()
        {
            return productStock.Keys.ToArray();
        }
    }
}
