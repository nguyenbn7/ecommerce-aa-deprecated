using System.Runtime.Serialization;

namespace Ecommerce.Module.Orders.Model;

public enum Status
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Payment Received")]
    PaymentReceived,
    [EnumMember(Value = "Payment Failed")]
    PaymentFailed
}