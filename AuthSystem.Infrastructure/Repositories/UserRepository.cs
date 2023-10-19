using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Interfaces;
using AuthSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly AuthSystemDbContext _context;

    public UserRepository(AuthSystemDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
