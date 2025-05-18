namespace Project.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string email)
            : base($"Email '{email}' is already in use.") { }
    }

    public class NicknameAlreadyExistsException : Exception
    {
        public NicknameAlreadyExistsException(string nickname)
            : base($"Nickname '{nickname}' is already taken.") { }
    }
    public class SubscriptionConfirmationNotFoundException : Exception
    {
        public SubscriptionConfirmationNotFoundException(int subscriptionId) : base(
            $"Subscription confirmation not found with given subscribe id {subscriptionId}.") { }
    }
    public class SubscriptionsNotFoundException : Exception
    {
        public SubscriptionsNotFoundException(int subscriptionId) : base($"Subscription not found with given Id {subscriptionId}.") { }
    }
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int userId) : base($"User with id {userId} does not exists.") { }
    }
    
    public class WatchHistoryNotFoundException : Exception
    {
        public WatchHistoryNotFoundException(int userId) : base($"Watch history not found with given userId {userId}.") { } 
    }
    
    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException(string title)
            : base($"Movie with title '{title}' does not exist.") { }
    }

    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException(int reviewId)
            : base($"Review with ID {reviewId} was not found.") { }
    }

    public class DuplicateReviewException : Exception
    {
        public DuplicateReviewException(string nickname, string mediaTitle)
            : base($"{nickname} has already reviewed '{mediaTitle}'.") { }
    }

    public class InvalidUserStatusException : Exception
    {
        public InvalidUserStatusException(string status)
            : base($"Invalid user status: '{status}'.") { }
    }

    public class InvalidGenreException : Exception
    {
        public InvalidGenreException(string genre)
            : base($"Genre '{genre}' is not a valid genre.") { }
    }

    public class StreamingServiceNotFoundException : Exception
    {
        public StreamingServiceNotFoundException(int serviceId)
            : base($"Streaming service with ID {serviceId} does not exist.") { }
    }

    public class SubtitleOptionNotFoundException : Exception
    {
        public SubtitleOptionNotFoundException(string mediaTitle)
            : base($"Subtitle option not found for media '{mediaTitle}'.") { }
    }

    public class AudioOptionNotFoundException : Exception
    {
        public AudioOptionNotFoundException(string mediaTitle)
            : base($"Audio option not found for media '{mediaTitle}'.") { }
    }

    public class WatchHistoryAlreadyExistsException : Exception
    {
        public WatchHistoryAlreadyExistsException(int userId, string mediaTitle)
            : base($"Watch history already exists for user {userId} and media '{mediaTitle}'.") { }
    }

    public class SubscriptionAlreadyExistsException : Exception
    {
        public SubscriptionAlreadyExistsException(int userId, int serviceId)
            : base($"User {userId} already has an active subscription for service ID {serviceId}.") { }
    }
    public class NoChangesDetectedException : Exception
    {
        public NoChangesDetectedException() 
            : base("The provided data is identical to the current user information. No changes were made.") { }
    }
    public class NoSubscriptionExistsException : Exception
    {
        public NoSubscriptionExistsException() 
            : base("There is no subscription exists in database.") { }
    }
   
    
    public class UserReviewNotFoundToMediaContentException : Exception 
    {
        public UserReviewNotFoundToMediaContentException(int userId, string mediaTitle)
            : base($"User with ID {userId} has not submitted a review for '{mediaTitle}'.") { }
    }
}