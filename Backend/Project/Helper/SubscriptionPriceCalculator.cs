using Project.Models;
using Project.Models.Enumerations;

namespace Project.Helper;

public static class SubscriptionPriceCalculator
{
    public static double CalculateAmount(double defaultPrice, User user)
    {
        double discount = 0;

        switch (user.Status)
        {
            case Status.Student: discount += 0.20; break;
            case Status.Elder: discount += 0.10; break;
        }

        switch (user.Country.Trim().ToUpperInvariant())
        {
            case "POLAND": discount += 0.05; break;
            case "GERMANY": discount += 0.03; break;
            case "FRANCE": discount += 0.02; break;
        }

        return Math.Max(defaultPrice * (1 - discount), 0);
    }
}
