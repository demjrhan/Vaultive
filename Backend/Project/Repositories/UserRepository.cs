using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs;
using Project.Exceptions;
using Project.Models;

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
    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateUserAsync(User existing, UpdateUserDTO updatedUser)
    {
        existing.Firstname = updatedUser.Firstname;
        existing.Lastname = updatedUser.Lastname;
        existing.Nickname = updatedUser.Nickname;
        existing.Email = updatedUser.Email;
        existing.Country = updatedUser.Country;
        existing.Status = updatedUser.Status;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new UserNotFoundException(userId);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

}