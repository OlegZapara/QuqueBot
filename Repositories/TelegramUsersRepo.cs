using Microsoft.EntityFrameworkCore;
using QuqueBot.Data;
using QuqueBot.Interfaces;
using QuqueBot.Models;
using Telegram.Bot.Types;

namespace QuqueBot.Repositories;

public class TelegramUsersRepo : ITelegramUsersRepo
{
    private readonly DataContext _dataContext;
    
    public TelegramUsersRepo(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<TelegramUser?> GetAsync(long id)
    {
        return await _dataContext.Users
            .Include(x => x.Queues)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TelegramUser> GetOrCreateAsync(User user)
    {
        var existingUser = await _dataContext.Users
            .Include(x => x.Queues)
            .SingleOrDefaultAsync(x => x.Id == user.Id);
        if (existingUser is not null)
        {
            return existingUser;
        }
        existingUser = new TelegramUser()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Queues = new List<Queue>(),
            Rating = new UserRating(user.Id)
        };
        _dataContext.Update(existingUser);
        await _dataContext.SaveChangesAsync();

        return existingUser;
    }

    public async Task<IEnumerable<TelegramUser>> GetAllAsync()
    {
        return await _dataContext.Users.ToListAsync();
    }

    public async Task<long> CreateAsync(TelegramUser user)
    {
        await _dataContext.Users.AddAsync(user);
        await _dataContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task UpdateAsync(TelegramUser user)
    {
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var existingUser = await GetAsync(id);
        if (existingUser is null)
        {
            throw new ArgumentException("User with this id does not exist");
        }
        _dataContext.Users.Remove(existingUser);
        await _dataContext.SaveChangesAsync();
    }
}