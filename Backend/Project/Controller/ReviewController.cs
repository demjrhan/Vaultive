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

        [HttpGet("All")]
        public async Task<IActionResult> GetAllReviewsWithMediaTitlesAsync()
        {
            try
            {
                var result = await _reviewService.GetAllReviewsWithMediaTitlesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Get/{reviewId:int}")]
        public async Task<IActionResult> GetReviewByIdAsync(int reviewId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewByIdAsync(reviewId);
                return Ok(reviews);
            }
            catch (ReviewNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
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
            catch (ReviewNotFoundException ex)
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
        public async Task<IActionResult> RemoveReviewAsync(int reviewId)
        {
            try
            {
                await _reviewService.RemoveReviewWithGivenIdAsync(reviewId);
                return Ok(new { message = "Review deleted successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ReviewNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }
    }
