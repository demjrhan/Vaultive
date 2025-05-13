namespace Project.DTOs;

public class UserWithSubscriptionsDTO
{
    public UserResponseDTO User { get; set; }
    public IEnumerable<SubscriptionResponseDTO> Subscriptions { get; set; } 
}