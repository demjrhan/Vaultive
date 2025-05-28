using Microsoft.AspNetCore.Mvc;
using Project.DTOs.ReviewDTOs;
using Project.Exceptions;
using Project.Services;

namespace Project.Controller;

    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        
        [HttpGet("Get/{reviewId:int}")]
        public async Task<IActionResult> GetReviewByIdAsync(int reviewId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewByIdAsync(reviewId);
                return Ok(reviews);
            }
            catch (ReviewDoesNotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }
        
        [HttpGet("All")]
        public async Task<IActionResult> GetAllReviewsAsync()
        {
            try
            {
                var result = await _reviewService.GetAllReviewsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("MediaContentsReviews/{mediaId:int}")]
        public async Task<IActionResult> GetReviewsOfMediaContentByIdAsync(int mediaId)
        {
            try
            {
                var result = await _reviewService.GetReviewsOfMediaContentByIdAsync(mediaId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> UpdateReviewAsync([FromBody] UpdateReviewDTO dto)
        {
            try
            {
                await _reviewService.UpdateReviewAsync(dto);
                return Ok(new { message = "Review updated successfully" });
            }
            catch (ReviewDoesNotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

        [HttpDelete("Remove/{reviewId:int}")]
        public async Task<IActionResult> DeleteReviewAsync(int reviewId)
        {
            try
            {
                await _reviewService.DeleteReviewWithGivenIdAsync(reviewId);
                return Ok(new { message = "Review deleted successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ReviewDoesNotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }
    }
