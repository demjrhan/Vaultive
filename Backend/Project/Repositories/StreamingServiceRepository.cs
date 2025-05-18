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

    public async Task<StreamingService?> GetStreamingServiceById(int streamingServiceId)
    {
       return await _context.StreamingServices.FirstOrDefaultAsync(s => s.Id == streamingServiceId);
    }
}