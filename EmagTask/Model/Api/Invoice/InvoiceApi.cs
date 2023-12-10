namespace EmagTask.Model.Api.Invoice
{
    public class InvoiceApi
    {
        public string Kind { get; set; }
        public string Number { get; set; }
        public string Sell_date { get; set; }
        public string Issue_date { get; set; }
        public string Payment_to { get; set; }
        public string Seller_name { get; set; }
        public string Seller_tax_no { get; set; }
        public string Buyer_post_code { get; set; }
        public string Buyer_city { get; set; }
        public string Buyer_street { get; set; }
        public string Buyer_country { get; set; }
        public string Buyer_name { get; set; }
        public bool Buyer_override { get; set; } = true;
        public InvoicePositionApi[] Positions { get; set; }
    }
}
