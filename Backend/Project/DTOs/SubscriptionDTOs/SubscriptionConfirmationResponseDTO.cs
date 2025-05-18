namespace Project.DTOs.SubscriptionDTOs;

public class SubscriptionConfirmationResponseDTO
{
    public int Id { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public decimal Price { get; set; }

    public int DurationInDays { get; set; }

    public int UserId { get; set; }

    public int SubscriptionId { get; set; }

    public string UserStatus { get; set; } = null!;
    public string UserCountry { get; set; } = null!;
    public string StreamingServiceName { get; set; } = null!;
}