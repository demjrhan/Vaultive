using System.ComponentModel.DataAnnotations.Schema;
using Project.Models.Enumerations;

namespace Project.Models;

public class SubscriptionConfirmation
{
    public int Id { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public double Price => CalculateAmount();

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = null!;
    
    private double CalculateAmount()
    {
        var basePrice = Subscription.DefaultPrice;
        double discount = 0;

        switch (User.Status)
        {
            case Status.Student:
                discount += 0.20; 
                break;
            case Status.Elder:
                discount += 0.10; 
                break;
        }

        switch (User.Country.Trim().ToUpperInvariant())
        {
            case "POLAND":
                discount += 0.05;
                break;
            case "GERMANY":
                discount += 0.03;
                break;
            case "FRANCE":
                discount += 0.02;
                break;
            default:
                discount += 0;
                break; 
        }

        var finalPrice = basePrice * (1 - discount);

        return Math.Max(finalPrice, 0);
    }

}
