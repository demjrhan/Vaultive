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

    public async Task<Review?> GetReviewOfUserToMediaContentAsync(int userId, int mediaId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.MediaContent)
            .Include(r => r.WatchHistory)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.MediaId == mediaId);
    }

    public async Task AddReviewAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
    }
    public async Task RemoveAsync(int reviewId)
    {
        var review = await _context.Reviews.FindAsync(reviewId);
        if (review == null) throw new ReviewDoesNotExistsException(reviewId);

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
   

    public async Task<IEnumerable<Review>> GetAllReviewsAsync()
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.MediaContent)
            .ThenInclude(r => r.WatchHistories)
            .OrderBy(r => r.UserId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsOfMediaContentByIdAsync(int mediaId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.MediaContent)
            .ThenInclude(r => r.WatchHistories)
            .Where(r => r.MediaId == mediaId)
            .ToListAsync();
    }


}