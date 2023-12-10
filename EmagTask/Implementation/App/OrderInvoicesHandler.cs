using EmagTask.Implementation.Invoice;
using EmagTask.Implementation.Order;
using EmagTask.Interface.App;
using EmagTask.Interface.Provider;
using EmagTask.Model.Api.Invoice;
using EmagTask.Model.Api.Order.Product;
using EmagTask.Model.Api.Order;
using EmagTask.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmagTask.Implementation.App
{
    public class OrderInvoicesHandler : IOrderInvoicesHandler
    {
        private readonly IInvoiceProvider invoiceProvider;
        private readonly IOrderProvider orderProvider;

        public OrderInvoicesHandler(AppSettings appSettings)
        {
            invoiceProvider = new InvoiceProvider(appSettings.InvoiceApiConfig);
            orderProvider = new OrderProvider(appSettings.OrderApiConfig);
        }

        public async Task HandleOrdersInvoices()
        {
            //Get all orders to process
            List<OrderApi> ordersToProcess = await orderProvider.GetOrdersToProcess(OrderStatus.Prepared);

            //Get all orders products
            Dictionary<long, HashSet<long>> ordersVsProductsIds = ordersToProcess.ToDictionary(x => x.Id, x => x.Products.Select(p => p.Id).ToHashSet());

            Dictionary<long, List<Product>> orderIdVsOrderProducts = await orderProvider.GetOrdersProducts(ordersVsProductsIds);

            //Prepare Invoice object for each order
            Dictionary<long, InvoiceApi> orderIdVsInvoice = GetInvoicesForOrders(ordersToProcess, orderIdVsOrderProducts);

            //Create orders Invoices
            Dictionary<long, string> orderIdVsInvoiceUrl = await GetInvoicesUrlsForOrders(orderIdVsInvoice);

            //Add Invoices to orders
            orderProvider.AddInvoiceFilesToOrders(orderIdVsInvoiceUrl);
        }

        private Dictionary<long, InvoiceApi> GetInvoicesForOrders(List<OrderApi> ordersToProcess, Dictionary<long, List<Product>> orderIdVsOrderProducts)
        {
            Dictionary<long, InvoiceApi> orderVsInvoice = new Dictionary<long, InvoiceApi>();

            foreach (OrderApi order in ordersToProcess)
            {
                InvoiceApi invoice = new InvoiceApi
                {
                    Number = order.Id.ToString(),
                    Sell_date = order.Date.ToString("yyyy-MM-dd"),
                    Issue_date = order.Date.ToString("yyyy-MM-dd"),
                    Payment_to = order.Date.ToString("yyyy-MM-dd"),
                    Buyer_post_code = order.Customer.Billing_postal_code,
                    Buyer_city = order.Customer.Billing_city,
                    Buyer_street = order.Customer.Billing_street,
                    Buyer_country = order.Customer.Billing_country,
                    Buyer_name = order.Customer.Name
                };

                List<InvoicePositionApi> invoicePostitions = new List<InvoicePositionApi>();

                foreach (OrderPosition orderPosition in order.Products)
                {
                    Product product = orderIdVsOrderProducts[order.Id].First(x => x.Id == orderPosition.Id);

                    InvoicePositionApi invoicePosition = new InvoicePositionApi
                    {
                        Name = product.Name,
                        Quantity = orderPosition.Quantity,
                        Total_price_gross = orderPosition.Sale_price,
                        Tax = order.Vat_value
                    };

                    invoicePostitions.Add(invoicePosition);
                }

                orderVsInvoice.Add(order.Id, invoice);
            }

            return orderVsInvoice;
        }

        private async Task<Dictionary<long, string>> GetInvoicesUrlsForOrders(Dictionary<long, InvoiceApi> orderIdVsInvoice)
        {
            Dictionary<long, string> orderIdVsInvoiceUrl = new Dictionary<long, string>();

            foreach (var orderInvoice in orderIdVsInvoice)
            {
                long invoiceId = await invoiceProvider.AddInvoice(orderInvoice.Value);
                string invoiceUrl = invoiceProvider.GetInvoiceFileUrl(invoiceId);
                orderIdVsInvoiceUrl.Add(orderInvoice.Key, invoiceUrl);
            }

            return orderIdVsInvoiceUrl;
        }
    }
}
