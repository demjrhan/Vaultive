using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Exceptions;
using Project.Models;

namespace Project.Repositories;

public class ReviewRepository
{
    private readonly MasterContext _context;

    public ReviewRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

   
    public async Task<IEnumerable<Review>> GetReviewsOfUserIdAsync(int userId)
    {
        return await _context.Reviews
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .Include(r => r.MediaContent)
            .Include(r => r.WatchHistory)
            .ToListAsync();
    }

    public async Task<Review?> GetReviewOfUserToMediaContentAsync(int userId, string mediaTitle)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.MediaContent)
            .Include(r => r.WatchHistory)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.MediaTitle == mediaTitle);
    }

    public async Task AddReviewAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
    }
    public async Task UpdateReviewAsync(Review updatedReview)
    {
        var existingReview = await _context.Reviews.FindAsync(updatedReview.Id);
        if (existingReview == null) throw new ReviewNotFoundException(updatedReview.Id);

        existingReview.Comment = updatedReview.Comment;
    }
    public async Task DeleteReviewAsync(int reviewId)
    {
        var review = await _context.Reviews.FindAsync(reviewId);
        if (review == null) throw new ReviewNotFoundException(reviewId);

        _context.Reviews.Remove(review);
    }
    public async Task<Review?> GetReviewByIdAsync(int reviewId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.MediaContent)
            .Include(r => r.WatchHistory)
            .FirstOrDefaultAsync(r => r.Id == reviewId);
    }
    public async Task<IEnumerable<Review>> GetReviewsForMediaAsync(string mediaTitle)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.WatchHistory)
            .Where(r => r.MediaTitle == mediaTitle)
            .ToListAsync();
    }
   


}