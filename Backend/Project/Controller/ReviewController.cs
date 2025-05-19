using Microsoft.AspNetCore.Mvc;
using Project.DTOs.ReviewDTOs;
using Project.Services;

namespace Project.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("media/{mediaTitle}")]
        public async Task<IActionResult> GetReviewsForMedia(string mediaTitle)
        {
            var reviews = await _reviewService.GetReviewsForMediaAsync(mediaTitle);
            return Ok(reviews);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReviewsByUser(int userId)
        {
            var reviews = await _reviewService.GetReviewsOfUserAsync(userId);
            return Ok(reviews);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDTO dto)
        {
            await _reviewService.AddReviewAsync(dto);
            return Ok(new { message = "Review added successfully" });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateReview([FromBody] UpdateReviewDTO dto)
        {
            await _reviewService.UpdateReviewAsync(dto);
            return Ok(new { message = "Review updated successfully" });
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            await _reviewService.DeleteReviewAsync(reviewId);
            return Ok(new { message = "Review deleted successfully" });
        }

    }
}