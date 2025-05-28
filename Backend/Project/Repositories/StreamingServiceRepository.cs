using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Exceptions;
using Project.Models;

namespace Project.Repositories;

public class StreamingServiceRepository
{
    private readonly MasterContext _context;

    public StreamingServiceRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

    public async Task<IEnumerable<StreamingService>> GetSupportedStreamingServicesOfMediaContent(string mediaTitle)
    {
        return await _context.StreamingServices
            .Include(ss => ss.Subscriptions)
            .ThenInclude(s => s.Confirmations)
            .Include(ss => ss.MediaContents)
            .Where(ss => ss.MediaContents.Any(m => m.Title == mediaTitle))
            .ToListAsync();
    }

    public async Task<StreamingService?> GetStreamingServiceByIdAsync(int streamingServiceId)
    {
        return await _context.StreamingServices.Include(ss => ss.Subscriptions)
            .ThenInclude(s => s.Confirmations)
            .ThenInclude(s => s.User)
            .Include(ss => ss.MediaContents)
            .FirstOrDefaultAsync(ss => ss.Id == streamingServiceId);
    }
    public async Task<IEnumerable<StreamingService>> GetAllStreamingServicesAsync()
    {
        return await _context.StreamingServices
            .Include(ss => ss.MediaContents)
            .Include(ss => ss.Subscriptions)
            .ThenInclude(s => s.Confirmations)
            .ToListAsync();

    }
    public async Task<StreamingService?> GetMostPopularStreamingServiceAsync()
    {
        return await _context.StreamingServices
            .Include(ss => ss.MediaContents)
            .Include(ss => ss.Subscriptions)
            .ThenInclude(s => s.Confirmations)
            .OrderByDescending(ss => ss.Subscriptions.Count)
            .FirstOrDefaultAsync();

    }
    public async Task RemoveAsync(int streamingServiceId)
    {
        var streamingService = await GetStreamingServiceByIdAsync(streamingServiceId);
        if (streamingService == null) throw new StreamingServiceDoesNotExistsException(new [] {streamingServiceId});
        _context.StreamingServices.Remove(streamingService);
    }
    public async Task AddAsync(StreamingService streamingService)
    {
        await _context.StreamingServices.AddAsync(streamingService);
    }
}