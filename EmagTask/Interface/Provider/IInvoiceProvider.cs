using EmagTask.Model.Api.Invoice;
using System.Threading.Tasks;

namespace EmagTask.Interface.Provider
{
    public interface IInvoiceProvider
    {
        Task<byte[]> GetInvoiceFile(long invoiceId);
        Task<long> AddInvoice(InvoiceApi invoice);
        string GetInvoiceFileUrl(long invoiceId);
    }
}
