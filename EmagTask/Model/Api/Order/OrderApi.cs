using System;

namespace EmagTask.Model.Api.Order
{
    public class OrderApi
    {
        public long Id { get; set; }
        public OrderStatus Status { get; set; }
        public bool IsComplete { get; set; }
        public decimal Vat_value { get; set; }
        public DateTime Date { get; set; }

        public OrderCustomer Customer { get; set; }
        public OrderPosition[] Products { get; set; }
        public OrderAttachment[] Attachments { get; set; }
    }
}
