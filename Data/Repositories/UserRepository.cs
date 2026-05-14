using LauncherHero.Starter.Models;
using Microsoft.EntityFrameworkCore;

namespace LauncherHero.Starter.Data.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.OrderBy(u => u.Id).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var existing = await GetByIdAsync(user.Id);
        if (existing is null)
            return false;

        existing.Name = user.Name;
        existing.Email = user.Email;
        if (!string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            existing.PasswordHash = user.PasswordHash;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await GetByIdAsync(id);
        if (existing is null)
            return false;

        _context.Users.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
