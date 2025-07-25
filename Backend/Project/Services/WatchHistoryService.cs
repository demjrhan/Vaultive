﻿using Project.Context;
using Project.DTOs.MediaContentDTOs;
using Project.DTOs.UserDTOs;
using Project.DTOs.WatchHistoryDTOs;
using Project.Exceptions;
using Project.Repositories;

namespace Project.Services;

public class WatchHistoryService
{
    private readonly ReviewRepository _reviewRepository;
    private readonly UserRepository _userRepository;
    private readonly MediaContentRepository _mediaContentRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;
    private readonly MasterContext _context;

    public WatchHistoryService(
        ReviewRepository reviewRepository,
        UserRepository userRepository,
        MediaContentRepository mediaContentRepository,
        WatchHistoryRepository watchHistoryRepository
    )
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _mediaContentRepository = mediaContentRepository;
        _watchHistoryRepository = watchHistoryRepository;
    }

    /* Get all watch histories. */
    public async Task<List<WatchHistoryDTO>> GetAllWatchHistoriesAsync()
    {
        var watchHistories = await _watchHistoryRepository.GetAllWatchHistories();

        return watchHistories.Select(wh => new WatchHistoryDTO()
        {
            Id = wh.Id,
            MediaId = wh.MediaId,
            MediaTitle = wh.MediaContent.Title,
            TimeLeftOf = wh.TimeLeftOf,
            WatchDate = wh.WatchDate
        }).ToList();
    }
    
    
    /* Get all watch histories with media content details. */
    public async Task<List<WatchHistoryDetailedDTO>> GetAllWatchHistoriesDetailedAsync()
    {
        var watchHistories = await _watchHistoryRepository.GetAllWatchHistories();

        return watchHistories.Select(wh => new WatchHistoryDetailedDTO()
        {
            Id = wh.Id,
            TimeLeftOf = wh.TimeLeftOf,
            WatchDate = wh.WatchDate,
            MediaContent = new MediaContentDTO()
            {
                Id = wh.MediaContent.Id,
                Title = wh.MediaContent.Title,
                Country = wh.MediaContent.Country,
                Description = wh.MediaContent.Description,
                Duration = wh.MediaContent.Duration,
                OriginalLanguage = wh.MediaContent.OriginalLanguage,
                ReleaseDate = wh.MediaContent.ReleaseDate,
            }
        }).ToList();
    }

    
    /* Get watch histories of the user with user details. */
    public async Task<List<UserWithWatchHistoriesDTO>> GetWatchHistoriesOfUserAsync(int userId)
    {
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");
        var watchHistories = await _watchHistoryRepository.GetWatchHistoriesOfUser(userId);

        return watchHistories.Select(wh => new UserWithWatchHistoriesDTO()
        {
            User = new UserDTO()
            {
                Id = wh.User.Id,
                Firstname = wh.User.Firstname,
                Lastname = wh.User.Lastname,
                Country = wh.User.Country,
                Nickname = wh.User.Nickname,
                Status = wh.User.Status.ToString()
            },
            WatchHistories = wh.User.WatchHistories.Select(uwh => new WatchHistoryDTO()
            {
                Id = uwh.Id,
                MediaId = uwh.MediaId,
                MediaTitle = uwh.MediaContent.Title,
                TimeLeftOf = uwh.TimeLeftOf,
                WatchDate = uwh.WatchDate
            }).ToList()
        }).ToList();
    }

    /* Method used in front end to determine if user can see the adding review button or not */
    public async Task<bool> CanUserWriteReviewAsync(int userId, int mediaId)
    {
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");
        if (mediaId <= 0) throw new ArgumentException("Media id can not be equal or smaller than 0.");

        if (await _mediaContentRepository.GetMediaContentWithGivenIdAsync(mediaId) == null)
            throw new MediaContentDoesNotExistsException(new[] { mediaId });

        if (await _userRepository.GetUserWithGivenId(userId) == null) throw new UserDoesNotExistsException(userId);

        var existingReview =
            await _reviewRepository.GetReviewOfUserToMediaContentAsync(userId, mediaId);

        if (existingReview != null) return false;

        var watchHistory = await _watchHistoryRepository.GetUsersWatchHistoryToMediaContent(userId, mediaId);
        return watchHistory != null;
    }
}