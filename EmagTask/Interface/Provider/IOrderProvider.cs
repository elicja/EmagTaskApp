using EmagTask.Model.Api.Order;
using EmagTask.Model.Api.Order.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmagTask.Interface.Provider
{
    public interface IOrderProvider
    {
        void AddInvoiceFilesToOrders(Dictionary<long, string> orderIdVsInvoiceUrl);
        Task<Dictionary<long, List<Product>>> GetOrdersProducts(Dictionary<long, HashSet<long>> ordersIdsVsProductsIds);
        Task<List<OrderApi>> GetOrdersToProcess(OrderStatus status);
    }
}
