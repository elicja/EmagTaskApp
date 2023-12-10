using Microsoft.AspNetCore.Routing.Internal;

namespace EmagTask.Model.Api.Order
{
    public class OrderAttachment
    {
        public long Order_id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public OrderAttachmentType Type { get; set; }
    }
}
