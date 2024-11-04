namespace ALMS.API.Data.Models
{
    public class Payment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required decimal Amount { get; set; }
        public required DateTime PaymentDate { get; set;}

        public required PaymentMethod PaymentMethod { get; set; }

        public required PaymentStatus PaymentStatus { get; set; }
    }

    public enum PaymentMethod
    {
        CreditCard = 0,
        Cash = 1,
        PayPal = 2,
        GooglePay = 3,
        ApplePay = 4
    }

    public enum PaymentStatus 
    { 
        pending = 0,
        Completed = 1, 
        Failed = 2
    }
}
