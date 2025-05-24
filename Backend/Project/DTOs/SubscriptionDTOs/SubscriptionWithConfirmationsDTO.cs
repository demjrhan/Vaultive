namespace Project.DTOs.SubscriptionDTOs;

public class SubscriptionWithConfirmationsDTO
{
    public SubscriptionDTO Subscription { get; set; } = new SubscriptionDTO();

    public ICollection<SubscriptionConfirmationDTO> Confirmations { get; set; } 
}