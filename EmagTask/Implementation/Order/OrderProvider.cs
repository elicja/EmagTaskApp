using EmagTask.Implementation.Api.Order;
using EmagTask.Interface.Api;
using EmagTask.Interface.Provider;
using EmagTask.Model.Api.Order;
using EmagTask.Model.Api.Order.Product;
using EmagTask.Model.Config;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmagTask.Implementation.Order
{
    public class OrderProvider : IOrderProvider
    {
        private readonly IOrderApiAdapter apiAdapter;

        public OrderProvider(OrderApiConfig orderApiConfig)
        {
            apiAdapter = new OrderApiAdapter(orderApiConfig);
        }

        public void AddInvoiceFilesToOrders(Dictionary<long, string> orderIdVsInvoiceUrl)
        {
            foreach (var attachmentData in orderIdVsInvoiceUrl)
            {
                OrderAttachment attachment = new OrderAttachment()
                {
                    Order_id = attachmentData.Key,
                    Name = $"{attachmentData.Key}.pdf",
                    Type = OrderAttachmentType.Invoice,
                };

                apiAdapter.AddInvoiceFileToOrder(attachment);
            }
        }

        public async Task<Dictionary<long, List<Product>>> GetOrdersProducts(Dictionary<long, HashSet<long>> ordersIdsVsProductsIds)
        {
            Dictionary<long, List<Product>> orderIdVsProducts = new Dictionary<long, List<Product>>();

            foreach (var orderIdVsProductsIds in ordersIdsVsProductsIds)
            {
                List<Product> products = new List<Product>();

                foreach (long productId in orderIdVsProductsIds.Value)
                {
                    Product product = await apiAdapter.GetProduct(productId);
                    products.Add(product);
                }

                orderIdVsProducts.Add(orderIdVsProductsIds.Key, products);
            }

            return orderIdVsProducts;
        }

        public async Task<List<OrderApi>> GetOrdersToProcess(OrderStatus status)
        {
            List<OrderApi> orders = await apiAdapter.GetOrders(status);
            List<OrderApi> filteredOrders = FilterOrders(orders);
            return filteredOrders;
        }

        private List<OrderApi> FilterOrders(List<OrderApi> orders)
        {
            List<OrderApi> filteredOrders = orders.Where
            (
                x => x.Status == OrderStatus.Prepared &&
                (
                    (x.Attachments == null || !x.Attachments.Any()) ||
                    (x.Attachments != null && x.Attachments.Any() && !x.Attachments.Any(a => a.Type == OrderAttachmentType.Invoice))
                )
             ).ToList();

            return filteredOrders;
        }
    }
}
