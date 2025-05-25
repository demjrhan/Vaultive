using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Models;

namespace Project.Repositories;

public class StreamingServiceRepository
{
    private readonly MasterContext _context;

    public StreamingServiceRepository(MasterContext masterContext)
    {
        _context = masterContext;
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
}