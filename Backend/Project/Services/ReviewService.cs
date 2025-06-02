using Project.Context;
using Project.DTOs.FrontendDTOs;
using Project.DTOs.ReviewDTOs;
using Project.Exceptions;
using Project.Repositories;

namespace Project.Services;

public class ReviewService
{
    private readonly ReviewRepository _reviewRepository;
    private readonly MasterContext _context;

    public ReviewService(
        MasterContext context,
        ReviewRepository reviewRepository
    )

    {
        _context = context;
        _reviewRepository = reviewRepository;
    }


    /* Delete review with given id. */
    public async Task DeleteReviewWithGivenIdAsync(int reviewId)
    {
        if (reviewId <= 0) throw new ArgumentException("Review id can not be equal or smaller than 0.");
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _reviewRepository.RemoveAsync(reviewId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /* Returns all the reviews. */
    public async Task<List<ReviewDTO>> GetAllReviewsAsync()
    {
        var reviews = await _reviewRepository.GetAllReviewsAsync();
        return reviews.Select(r => new ReviewDTO()
        {
            Id = r.Id,
            MediaTitle = r.MediaContent.Title,
            Comment = r.Comment,
            Nickname = r.User.Nickname,
            WatchedOn = r.WatchHistory.WatchDate,
        }).ToList();
    }

    /* Returns all reviews of given userId  */
    public async Task<List<ReviewDTO>> GetAllReviewsOfUserByIdAsync(int userId)
    {
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");
        var reviews = await _reviewRepository.GetReviewsOfUserIdAsync(userId);
        return reviews.Select(r => new ReviewDTO()
        {
            Id = r.Id,
            Comment = r.Comment,
            Nickname = r.User.Nickname,
            MediaTitle = r.MediaContent.Title,
            WatchedOn = r.WatchHistory.WatchDate,
        }).OrderBy(r => r.Nickname).ToList();
    }
    /* Returns all the reviews of media content.*/
    public async Task<List<ReviewFrontendDTO>> GetReviewsOfMediaContentByIdAsync(int mediaId)
    {
        if (mediaId <= 0) throw new ArgumentException("Media id can not be equal or smaller than 0.");

        var reviews = await _reviewRepository.GetReviewsOfMediaContentByIdAsync(mediaId);
        return reviews.Select(r => new ReviewFrontendDTO()
        {
            Id = r.Id,
            Comment = r.Comment,
            Nickname = r.User.Nickname,
            WatchedOn = r.WatchHistory.WatchDate,
        }).OrderBy(r => r.Nickname).ToList();
    }


    /* Returns review by id. */
    public async Task<ReviewDTO> GetReviewByIdAsync(int reviewId)
    {
        if (reviewId <= 0) throw new ArgumentException("Review id can not be equal or smaller than 0.");

        var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
        if (review == null) throw new ReviewDoesNotExistsException(reviewId);
        return new ReviewDTO()
        {
            Id = review.Id,
            Comment = review.Comment,
            MediaTitle = review.MediaContent.Title,
            Nickname = review.User.Nickname,
            WatchedOn = review.WatchHistory.WatchDate
        };
    }

    /* Updates the review with given information. */
    public async Task UpdateReviewAsync(UpdateReviewDTO reviewDto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (reviewDto.Id <= 0) throw new ArgumentException("Review id can not be equal or smaller than 0.");

            if (reviewDto == null)
                throw new ArgumentNullException(nameof(reviewDto));

            var review = await _reviewRepository.GetReviewByIdAsync(reviewDto.Id) ??
                         throw new ReviewDoesNotExistsException(reviewDto.Id);

            ValidateReview(comment: reviewDto.Comment);

            review.Comment = reviewDto.Comment;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    /* Validation operations for review comment */

    public void ValidateReview(string comment)
    {
        if (string.IsNullOrWhiteSpace(comment) || comment.Length < 2 || comment.Length > 100)
            throw new ArgumentException("Comment must be between 2 and 100 characters.", nameof(comment));


        if (comment.IndexOf("http://", StringComparison.OrdinalIgnoreCase) >= 0 ||
            comment.IndexOf("https://", StringComparison.OrdinalIgnoreCase) >= 0)
            throw new ArgumentException("Do not include links in your review.", nameof(comment));

        if (!comment.Any(char.IsLetter))
            throw new ArgumentException("Comment must include at least one letter.", nameof(comment));

        if (comment.Length > 5 && comment == comment.ToUpperInvariant())
            throw new ArgumentException("Please avoid writing the entire comment in uppercase.", nameof(comment));
    }
}