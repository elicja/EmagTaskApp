namespace EmagTask.Model.Api.Invoice
{
    public class InvoicePositionApi
    {
        public string Name { get; set; }
        public decimal Tax { get; set; }
        public decimal Total_price_gross { get; set; }
        public int Quantity { get; set; }
    }
}
