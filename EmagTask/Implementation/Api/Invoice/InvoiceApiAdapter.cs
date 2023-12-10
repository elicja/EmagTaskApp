using EmagTask.Implementation.Api.Extension;
using EmagTask.Implementation.App.Helper;
using EmagTask.Interface.Api;
using EmagTask.Model.Api.Invoice;
using EmagTask.Model.Config;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmagTask.Implementation.Api.Invoice
{
    public class InvoiceApiAdapter : IInvoiceApiAdapter
    {
        private readonly InvoiceApiConfig config;
        private readonly IApiProvider apiProvider;

        public InvoiceApiAdapter(InvoiceApiConfig invoiceApiAConfig)
        {
            config = invoiceApiAConfig;
            apiProvider = new ApiProvider(invoiceApiAConfig.InvoiceApiBaseUrl);
        }

        public async Task<long> AddInvoice(InvoiceApi invoice)
        {
            const string resource = "invoices.json";
            RestRequest request = apiProvider.GetRequest(resource, Method.Post);

            InvoiceRequestWrapperApi invoiceRequestObj = new InvoiceRequestWrapperApi(config.InvoiceApiKey, invoice);

            string serializedRequestObj = AppHelper.SerializeLowerCase(invoiceRequestObj);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(serializedRequestObj);

            RestResponse response = await apiProvider.ExecuteRequest(request);

            InvoiceOutcomeApi requestResult = response.Deserialize<InvoiceOutcomeApi>();

            return requestResult.Id;
        }

        public async Task<byte[]> GetInvoiceFile(long invoiceId)
        {
            string resource = $"invoices/{invoiceId}.pdf";

            RestRequest request = apiProvider.GetRequest(resource);

            byte[] file = await apiProvider.DownloadFile(request);

            return file;
        }

        public string GetInvoiceFileUrl(long invoiceId)
        {
            return $"{config.InvoiceApiBaseUrl}invoices/{invoiceId}.pdf?api_token={config.InvoiceApiKey}";
        }
    }
}
