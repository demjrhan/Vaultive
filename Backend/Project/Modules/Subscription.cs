namespace Project.Modules;

public class Subscription
{
    public int Id { get; set; }

    public double DefaultPrice { get; set; }
    public double DurationInDays { get; set; }

    public ICollection<SubscriptionConfirmation> Confirmations { get; set; } = new HashSet<SubscriptionConfirmation>();
    
    public int StreamingServiceId { get; set; }
    public StreamingService StreamingService { get; set; } = null!;
}
