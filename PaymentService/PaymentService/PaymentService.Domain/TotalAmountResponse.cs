namespace PaymentService.Domain
{
    public class TotalAmountResponse
    {
        public long OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
