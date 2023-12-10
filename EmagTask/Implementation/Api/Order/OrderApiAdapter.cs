using EmagTask.Implementation.Api.Extension;
using EmagTask.Implementation.App.Helper;
using EmagTask.Interface.Api;
using EmagTask.Model.Api.Order;
using EmagTask.Model.Api.Order.Product;
using EmagTask.Model.Config;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmagTask.Implementation.Api.Order
{
    public class OrderApiAdapter : IOrderApiAdapter
    {
        private readonly OrderApiConfig config;
        private readonly string apiKey;
        private readonly IApiProvider apiProvider;

        public OrderApiAdapter(OrderApiConfig config)
        {
            this.config = config;
            apiKey = GenerateApiKey(this.config.OrderApiUserName, this.config.OrderApiUserPassword);
            apiProvider = new ApiProvider(this.config.OrderApiBaseUrl);
        }

        public void AddInvoiceFileToOrder(OrderAttachment attachment)
        {
            string resource = "order/attachments.save";

            RestRequest request = apiProvider.GetRequest(resource, Method.Post);
            SetBaseRequestData(ref request);

            string serializeObject = AppHelper.SerializeLowerCase(attachment);
            request.AddJsonBody(serializeObject);

            apiProvider.ExecuteRequest(request);
        }

        public async Task<List<OrderApi>> GetOrders(OrderStatus status, int startPage = 1, int packageSize = 1000)
        {
            string resource = "order/read";

            int currentPage = startPage;
            int itemsPerPage = packageSize;
            bool areThereOrdersToImport = true;

            List<OrderApi> orders = new List<OrderApi>();

            while (areThereOrdersToImport)
            {
                RestRequest request = apiProvider.GetRequest(resource);
                SetBaseRequestData(ref request);

                request.AddParameter("currentPage", currentPage);
                request.AddParameter("itemsPerPage", itemsPerPage);
                request.AddParameter("status", status);

                RestResponse response = await apiProvider.ExecuteRequest(request);

                List<OrderApi> ordersFromRequest = response.Deserialize<List<OrderApi>>();
                orders.AddRange(ordersFromRequest);

                if (ordersFromRequest.Count() < itemsPerPage)
                {
                    areThereOrdersToImport = false;
                }

                currentPage += 1;
            }

            return orders;
        }

        public async Task<Product> GetProduct(long productId)
        {
            string resource = "product_offer.read";

            RestRequest request = apiProvider.GetRequest(resource);
            SetBaseRequestData(ref request);

            request.AddParameter("id", productId);

            RestResponse response = await apiProvider.ExecuteRequest(request);

            Product product = response.Deserialize<Product>();

            return product;
        }

        private string GenerateApiKey(string username, string password)
        {
            string apiKeyInputString = $"{username}:{password}";
            byte[] apiKeyInputStringAsBytes = System.Text.Encoding.UTF8.GetBytes(apiKeyInputString);
            return Convert.ToBase64String(apiKeyInputStringAsBytes);
        }

        private void SetBaseRequestData(ref RestRequest request)
        {
            request.AddHeader("Contect-Type", "application/json");
            request.AddHeader("Authorization", apiKey);
        }
    }
}
