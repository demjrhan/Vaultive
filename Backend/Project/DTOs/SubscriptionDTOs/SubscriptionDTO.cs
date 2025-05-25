namespace Project.DTOs.SubscriptionDTOs;

public class SubscriptionDTO
{
    public int Id { get; set; }

    public int DaysLeft { get; set; }
    public string StreamingServiceName { get; set; }
    
    public decimal AmountPaid { get; set; }
}