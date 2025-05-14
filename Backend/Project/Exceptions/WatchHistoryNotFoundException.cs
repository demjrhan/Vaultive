namespace Project.Exceptions;

public class WatchHistoryNotFoundException : Exception
{
    public WatchHistoryNotFoundException(int userId) : base($"Watch history not found with given userId {userId}.")
    {
    }
}