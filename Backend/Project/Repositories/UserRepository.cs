using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Modules;

namespace Project.Repositories;

public class UserRepository
{
    private readonly MasterContext _context;

    public UserRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

    public async Task<User?> GetUserWithIdAsync(int userId)
    {
        return await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
    }
}