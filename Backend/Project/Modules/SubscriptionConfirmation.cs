namespace Project.Modules;

public class SubscriptionConfirmation
{
    public int Id { get; set; }

    public string PaymentMethod { get; set; } = null!;
    public double Amount { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = null!;
}
