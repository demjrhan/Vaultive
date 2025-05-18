namespace Project.DTOs.SubscriptionDTOs;

public class SubscriptionResponseDTO
{
    public int Id { get; set; }

    public decimal Price { get; set; }
    public int DaysLeft { get; set; }
    public string StreamingServiceName { get; set; }
}