using EmagTask.Model.Api.Order;
using EmagTask.Model.Api.Order.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmagTask.Interface.Api
{
    public interface IOrderApiAdapter
    {
        void AddInvoiceFileToOrder(OrderAttachment attachment);
        Task<List<OrderApi>> GetOrders(OrderStatus status, int startPage = 1, int packageSize = 1000);
        Task<Product> GetProduct(long productId);
    }
}
