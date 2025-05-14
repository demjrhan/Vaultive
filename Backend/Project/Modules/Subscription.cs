using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Modules;

public class Subscription
{
    public int Id { get; set; }

    public double DefaultPrice { get; set; }
    
    [NotMapped]
    public int? DurationInDays
    {
        get
        {
            var latest = Confirmations.MaxBy(c => c.StartTime);
            return latest == null ? null : (int)(latest.EndTime - latest.StartTime).TotalDays;
        }
    }



    public ICollection<SubscriptionConfirmation> Confirmations { get; set; } = new HashSet<SubscriptionConfirmation>();
    
    public int StreamingServiceId { get; set; }
    public StreamingService StreamingService { get; set; } = null!;
}
