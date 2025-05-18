using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models;

public class Subscription
{
    public int Id { get; set; }

    public decimal DefaultPrice { get; set; }
    
    [NotMapped]
    public int DurationInDays
    {
        get
        {
            var latest = Confirmations.MaxBy(c => c.StartTime);
            return latest == null ? 0 : (int)(latest.EndTime - latest.StartTime).TotalDays;
        }
    }

    public ICollection<SubscriptionConfirmation> Confirmations { get; set; } = new HashSet<SubscriptionConfirmation>();
    
    public int StreamingServiceId { get; set; }
    public StreamingService StreamingService { get; set; } = null!;
}
