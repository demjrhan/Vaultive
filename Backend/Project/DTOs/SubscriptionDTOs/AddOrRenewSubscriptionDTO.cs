namespace Project.DTOs.SubscriptionDTOs;

public class AddOrRenewSubscriptionDTO
{
    
    public int StreamingServiceId { get; set; }
    public string PaymentMethod { get; set; } = null!;


    public int DurationInMonth { get; set; }
    public int UserId { get; set; }
}