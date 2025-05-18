using Project.Context;
using Project.DTOs.ReviewDTOs;
using Project.Exceptions;
using Project.Models;
using Project.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Project.Services;

public class ReviewService
{
    private readonly ReviewRepository _reviewRepository;
    private readonly UserRepository _userRepository;
    private readonly MovieRepository _movieRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;
    private readonly MasterContext _context;

    public ReviewService(
        MasterContext context,
        ReviewRepository reviewRepository,
        UserRepository userRepository,
        MovieRepository movieRepository,
        WatchHistoryRepository watchHistoryRepository)
    {
        _context = context;
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _movieRepository = movieRepository;
        _watchHistoryRepository = watchHistoryRepository;
    }

    public async Task<List<ReviewResponseDTO>> GetReviewsForMediaAsync(string mediaTitle)
    {
        var reviews = await _reviewRepository.GetReviewsForMediaAsync(mediaTitle);

        return reviews.Select(r => new ReviewResponseDTO
        {
            Id = r.Id,
            Rating = r.Rating,
            Comment = r.Comment,
            Username = r.User.Nickname,
            MediaTitle = r.MediaTitle,
            WatchedOn = r.WatchHistory?.WatchDate.ToString("yyyy-MM-dd")
        }).ToList();
    }

    public async Task<List<ReviewResponseDTO>> GetReviewsOfUserAsync(int userId)
    {
        var reviews = await _reviewRepository.GetReviewsOfUserIdAsync(userId);

        return reviews.Select(r => new ReviewResponseDTO
        {
            Id = r.Id,
            Rating = r.Rating,
            Comment = r.Comment,
            MediaTitle = r.MediaTitle,
            WatchedOn = r.WatchHistory?.WatchDate.ToString("yyyy-MM-dd")
        }).ToList();
    }

    public async Task AddReviewAsync(AddReviewDTO dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var user = await _userRepository.GetUserWithIdAsync(dto.UserId)
                       ?? throw new UserNotFoundException(dto.UserId);

            var media = await _movieRepository.GetMovieByTitleAsync(dto.MediaTitle)
                        ?? throw new MovieNotFoundException(dto.MediaTitle);

            var watchHistory = await _watchHistoryRepository.GetByUserAndMediaAsync(dto.UserId, dto.MediaTitle);

            var review = new Review
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                UserId = user.Id,
                User = user,
                MediaTitle = dto.MediaTitle,
                MediaContent = media,
                WatchHistory = watchHistory
            };

            await _reviewRepository.AddReviewAsync(review);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateReviewAsync(UpdateReviewDTO dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var updatedReview = new Review
            {
                Id = dto.Id,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            await _reviewRepository.UpdateReviewAsync(updatedReview);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteReviewAsync(int reviewId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _reviewRepository.DeleteReviewAsync(reviewId);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<double?> GetAverageRatingForMediaAsync(string mediaTitle)
    {
        return await _reviewRepository.GetAverageRatingForMediaAsync(mediaTitle);
    }
}
