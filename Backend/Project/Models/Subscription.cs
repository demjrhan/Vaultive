using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models;

public class Subscription
{
    public int Id { get; set; }

    
    [NotMapped]
    public int DurationInDays
    {
        get => CalculateDurationInDays();
    }

    private int CalculateDurationInDays()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var latest = Confirmations.MaxBy(c => c.StartTime);
        if (latest == null)
            return 0;

        return latest.EndTime < today
            ? 0
            : latest.EndTime.DayNumber
              - today.DayNumber
              + 1;
    }


    public ICollection<SubscriptionConfirmation> Confirmations { get; set; } = new List<SubscriptionConfirmation>();
    
    public int StreamingServiceId { get; set; }
    public StreamingService StreamingService { get; set; } = null!;
}
