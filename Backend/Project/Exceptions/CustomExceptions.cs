namespace Project.Exceptions
{
    /* media Content */
    public class MediaContentDoesNotExistsException : Exception
    {
        public MediaContentDoesNotExistsException(IEnumerable<int> mediaContentIds)
            : base(GenerateMessage(mediaContentIds))
        {
        }
        private static string GenerateMessage(IEnumerable<int> mediaContentIds)
        {
            var ids = mediaContentIds as IList<int> ?? mediaContentIds.ToList();
            return ids.Count == 1
                ? $"MediaContent with id: '{ids.First()}' does not exist."
                : $"MediaContents with IDs [{string.Join(", ", ids)}] do not exist.";
        }
    }

    public class NoMediaContentFoundException : Exception
    {
        public NoMediaContentFoundException(string text)
            : base($"Did not manage to find any media content which contains {text} in the title.")
        {
        }
    }

    public class MediaContentTitleMustBeUniqueException : Exception
    {
        public MediaContentTitleMustBeUniqueException(string title)
            : base($"{title} already exists.")
        {
        }
    }

    public class MediaContentIsNotPublishedException : Exception
    {
        public MediaContentIsNotPublishedException()
            : base("Media content state is not published, no action can be done.")
        {
        }
    }

    public class InvalidGenreException : Exception
    {
        public InvalidGenreException(string genre)
            : base($"Genre '{genre}' is not a valid genre.")
        {
        }
    }

    public class AtLeastOneGenreMustExistsException : Exception
    {
        public AtLeastOneGenreMustExistsException()
            : base("At least one genre must be existing in the media content.")
        {
        }
    }

    public class AtLeastOneTopicMustExistsException : Exception
    {
        public AtLeastOneTopicMustExistsException()
            : base("At least one topic must be existing in the media content.")
        {
        }
    }

    public class InvalidTopicException : Exception
    {
        public InvalidTopicException(string topic)
            : base($"Topic '{topic}' is not a valid topic.")
        {
        }
    }

    public class InvalidStateException : Exception
    {
        public InvalidStateException(string state)
            : base($"State '{state}' is not a valid state.")
        {
        }
    }

    /* user */
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string email)
            : base($"Email '{email}' is already in use.")
        {
        }
    }

    public class NicknameAlreadyExistsException : Exception
    {
        public NicknameAlreadyExistsException(string nickname)
            : base($"Nickname '{nickname}' already exists.")
        {
        }
    }

    public class UserDoesNotExistsException : Exception
    {
        public UserDoesNotExistsException(int userId)
            : base($"User with id {userId} does not exists.")
        {
        }
    }

    public class InvalidUserStatusException : Exception
    {
        public InvalidUserStatusException(string status)
            : base($"Invalid user status: '{status}'.")
        {
        }
    }

    public class NoChangesDetectedException : Exception
    {
        public NoChangesDetectedException()
            : base("The provided data is identical to the current data on database. No changes were made.")
        {
        }
    }

    public class UserCanNotWatchMediaContentException : Exception
    {
        public UserCanNotWatchMediaContentException(int userId)
            : base($"{userId} can not watch the media content, does not satisfy the required subscriptions for streaming services.")
        {
        }
    }

    public class UserHasNoActiveSubscriptionException : Exception
    {
        public UserHasNoActiveSubscriptionException(string nickname, string? message = "")
            : base($"User {nickname} does not have any active subscription. {message}")
        {
        }
    }

    /* watch history */
    public class WatchHistoryDoesNotExistsException : Exception
    {
        public WatchHistoryDoesNotExistsException(int userId)
            : base($"Watch history not found with given userId {userId}.")
        {
        }
    }

    public class MediaContentAlreadyWatchedException : Exception
    {
        public MediaContentAlreadyWatchedException(string nickname, string mediaTitle)
            : base($"{nickname} already watched and finished media '{mediaTitle}'.")
        {
        }
    }
    public class MovieDoesNotExistsException : Exception
    {
        public MovieDoesNotExistsException(int movieId)
            : base($"Movie with id '{movieId}' does not exist.")
        {
        }
    }

    public class ShortFilmDoesNotExistsException : Exception
    {
        public ShortFilmDoesNotExistsException(int movieId)
            : base($"Short film with id '{movieId}' does not exist.")
        {
        }
    }

    public class DocumentaryDoesNotExistsException : Exception
    {
        public DocumentaryDoesNotExistsException(int movieId)
            : base($"Documentary film with id '{movieId}' does not exist.")
        {
        }
    }

    /* subscriptions */
    public class SubscriptionsDoesNotExistsException : Exception
    {
        public SubscriptionsDoesNotExistsException(int subscriptionId)
            : base($"Subscription not found with given Id {subscriptionId}.")
        {
        }
    }

    public class SubscriptionAlreadyExistsException : Exception
    {
        public SubscriptionAlreadyExistsException(int userId, int serviceId)
            : base($"User {userId} already has an active subscription for service ID {serviceId}.")
        {
        }
    }

    /* streaming service */
    public class NoStreamingServiceExistsException : Exception
    {
        public NoStreamingServiceExistsException()
            : base("There is no streaming service exists in database.")
        {
        }
    }

    public class StreamingServiceDoesNotExistsException : Exception
    {
        public StreamingServiceDoesNotExistsException(IEnumerable<int> serviceIds)
            : base(GenerateMessage(serviceIds))
        {
        }

        private static string GenerateMessage(IEnumerable<int> serviceIds)
        {
            var ids = serviceIds as IList<int> ?? serviceIds.ToList();
            return ids.Count == 1
                ? $"Streaming service with ID {ids.First()} does not exist."
                : $"Streaming services with IDs [{string.Join(", ", ids)}] do not exist.";
        }
    }

    public class StreamingServiceNameMustBeUniqueException : Exception
    {
        public StreamingServiceNameMustBeUniqueException(string name)
            : base($"{name} already exists.")
        {
        }
    }

    /* Option */
    public class AtLeastOneOptionMustExistsException : Exception
    {
        public AtLeastOneOptionMustExistsException()
            : base("There must be at least one option existing in media content. Either audio or subtitle.")
        {
        }
    }

    /* review */
    public class ReviewDoesNotExistsException : Exception
    {
        public ReviewDoesNotExistsException(int reviewId)
            : base($"Review with ID {reviewId} was not found.")
        {
        }
    }

    public class DuplicateReviewException : Exception
    {
        public DuplicateReviewException(string nickname, string mediaTitle)
            : base($"{nickname} has already reviewed '{mediaTitle}'.")
        {
        }
    }

    /* other */
    public class PaymentFailedException : Exception
    {
        public PaymentFailedException()
            : base("Payment was declined by the bank.")
        {
        }
    }

    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message)
            : base(message)
        {
        }
    }
}
