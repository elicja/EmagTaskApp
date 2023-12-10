namespace EmagTask.Model.Api.Order
{
    public class OrderCustomer
    {
        public string Name { get; set; }
        public string Billing_phone { get; set; }
        public string Billing_country { get; set; }
        public string Billing_city { get; set; }
        public string Billing_street { get; set; }
        public string Billing_postal_code { get; set; }
    }
}
