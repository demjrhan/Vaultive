using System.ComponentModel.DataAnnotations.Schema;
using Project.Models.Enumerations;

namespace Project.Models;

public class SubscriptionConfirmation
{
    public int Id { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public decimal Price { get; set; }
    public DateOnly StartTime { get; set; }
    public DateOnly EndTime { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = null!;
}