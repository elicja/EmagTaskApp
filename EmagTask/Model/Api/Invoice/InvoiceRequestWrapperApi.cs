namespace EmagTask.Model.Api.Invoice
{
    public class InvoiceRequestWrapperApi
    {
        public string Api_token { get; set; }
        public InvoiceApi Invoice { get; set; }

        public InvoiceRequestWrapperApi() { }

        public InvoiceRequestWrapperApi(string api_token, InvoiceApi invoice)
        {
            Api_token = api_token;
            Invoice = invoice;
        }
    }
}
