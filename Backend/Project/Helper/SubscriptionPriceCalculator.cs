using Project.Models;
using Project.Models.Enumerations;

namespace Project.Helper;

public static class SubscriptionPriceCalculator
{
    public static decimal CalculateAmount(decimal defaultPrice, User user)
    {
        decimal discount = 0;

        switch (user.Status)
        {
            case Status.Student: discount += 0.20m; break;
            case Status.Elder: discount += 0.10m; break;
        }

        switch (user.Country.Trim().ToUpperInvariant())
        {
            case "POLAND": discount += 0.05m; break;
            case "GERMANY": discount += 0.03m; break;
            case "FRANCE": discount += 0.02m; break;
        }

        return Math.Max(defaultPrice * (1 - discount), 0);
    }
}