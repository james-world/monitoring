using System;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Features.Orders
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ProductCatalog catalog;

        public OrdersController(ProductCatalog catalog)
        {
            this.catalog = catalog;
        }

        [HttpPost]
        public IActionResult PlaceOrder(NewOrder order)
        {
            if (!catalog.IsInCatalog(order.Product))
                return NotFound($"Product {order.Product} is not in the catalog.");

            if (!catalog.TryPlaceOrder(order.Product, order.Amount))
                return Conflict($"Stock not available for product {order.Product}.");

            return Accepted((object)$"Order for {order.Amount} × {order.Product} accepted.");
        }
    }
}
