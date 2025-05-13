namespace backend.Exceptions;

public class SubscriptionsNotFoundException: System.Exception
{
    public SubscriptionsNotFoundException(int userId) : base($"Subscription not found with given userId {userId}.")
    {
        
    }
}