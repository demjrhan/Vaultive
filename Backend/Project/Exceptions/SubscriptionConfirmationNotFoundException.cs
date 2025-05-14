namespace Project.Exceptions;

public class SubscriptionConfirmationNotFoundException: Exception
{
    public SubscriptionConfirmationNotFoundException(int subscriptionId) : base($"Subscription confirmation not found with given subscribe id {subscriptionId}.")
    {
        
    }
}