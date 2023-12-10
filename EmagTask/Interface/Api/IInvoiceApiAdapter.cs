using EmagTask.Model.Api.Invoice;
using System.Threading.Tasks;

namespace EmagTask.Interface.Api
{
    public interface IInvoiceApiAdapter
    {
        Task<long> AddInvoice(InvoiceApi invoice);
        Task<byte[]> GetInvoiceFile(long invoiceId);
        string GetInvoiceFileUrl(long invoiceId);
    }
}
