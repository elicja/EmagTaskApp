using EmagTask.Implementation.Api.Invoice;
using EmagTask.Interface.Api;
using EmagTask.Interface.Provider;
using EmagTask.Model.Api.Invoice;
using EmagTask.Model.Config;
using System;
using System.Threading.Tasks;

namespace EmagTask.Implementation.Invoice
{
    public class InvoiceProvider : IInvoiceProvider
    {
        private readonly IInvoiceApiAdapter apiAdapter;
        private readonly string sellerName;
        private readonly string sellerTaxNumber;
        private readonly string invoiceKind;

        public InvoiceProvider(InvoiceApiConfig invoiceApiConfig)
        {
            apiAdapter = new InvoiceApiAdapter(invoiceApiConfig);
            sellerName = invoiceApiConfig.InvoiceSellerName;
            sellerTaxNumber = invoiceApiConfig.InvoiceSellerTaxNumber;
            invoiceKind = invoiceApiConfig.InvoiceKind;
        }

        public async Task<long> AddInvoice(InvoiceApi invoice)
        {
            invoice.Seller_tax_no = sellerTaxNumber;
            invoice.Seller_name = sellerName;
            invoice.Kind = invoiceKind;
            invoice.Buyer_override = true;

            long invoiceId = await apiAdapter.AddInvoice(invoice);
            return invoiceId;
        }

        public async Task<byte[]> GetInvoiceFile(long invoiceId)
        {
            return await apiAdapter.GetInvoiceFile(invoiceId);
        }

        public string GetInvoiceFileUrl(long invoiceId)
        {
            return apiAdapter.GetInvoiceFileUrl(invoiceId);
        }
    }
}
