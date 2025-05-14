namespace Project.Exceptions;

public class SubscriptionsNotFoundException: Exception
{
    public SubscriptionsNotFoundException(int userId) : base($"Subscription not found with given userId {userId}.")
    {
        
    }
}