namespace ManagementTrackingSystem.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public decimal Amount { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string Status { get; set; }
        public string ShippedDate { get; set; }
        public AddressDTO ShippingAddress { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public TrackingDetailDTO TrackingDetail { get; set; }
    }
}
