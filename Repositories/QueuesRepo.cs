using Microsoft.EntityFrameworkCore;
using QuqueBot.Data;
using QuqueBot.Interfaces;
using QuqueBot.Models;

namespace QuqueBot.Repositories;

public class QueuesRepo : IQueuesRepo
{
    private readonly DataContext _dataContext;
    
    public QueuesRepo(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<Queue?> GetAsync(long id)
    {
        var queue =  await _dataContext.Queues
            .Include(x => x.Users)
            .SingleOrDefaultAsync(x => x.Id == id);
        if (queue is null)
        {
            return null;
        }
        queue.Users = queue.Users
            .OrderBy(x => _dataContext.QueueTelegramUser.Find(queue.Id, x.Id))
            .ToList();
        return queue;
    }

    public async Task<Queue?> GetByMessageIdAsync(long id)
    {
        var queue =  await _dataContext.Queues
            .Include(x => x.Users)
            .SingleOrDefaultAsync(x => x.MessageId == id);
        if (queue is null)
        {
            return null;
        }
        queue.Users = queue.Users
            .OrderBy(x => _dataContext.QueueTelegramUser.Find(queue.Id, x.Id))
            .ToList();
        return queue;
    }

    public async Task<IEnumerable<Queue>> GetAllAsync()
    {
        return await _dataContext.Queues.ToListAsync();
    }

    public async Task ShuffleAsync(Queue queue)
    {
        var relations = _dataContext.QueueTelegramUser
            .Where(x => x.QueuesId == queue.Id);
        // TODO: fix this shit
    }

    public async Task<long> CreateAsync(Queue queue)
    {
        queue.Id = default;
        await _dataContext.Queues.AddAsync(queue);
        await _dataContext.SaveChangesAsync();
        return queue.Id;
    }

    public async Task UpdateAsync(Queue queue)
    {
        _dataContext.Queues.Update(queue);
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var existingQueue = await GetAsync(id);
        if (existingQueue is null)
        {
            throw new ArgumentException("Element with this id does not exist");
        }
        _dataContext.Remove(existingQueue);
        await _dataContext.SaveChangesAsync();
    }

    public async Task JoinAsync(Queue queue, TelegramUser user)
    {
        queue.Users.Add(user);
        _dataContext.Queues.Update(queue);
        var lastUserIndex = queue.Users.Count;
        var queueUserRelation = new QueueTelegramUser()
        {
            UsersId = user.Id,
            QueuesId = queue.Id,
            Position = lastUserIndex
        };
        await _dataContext.QueueTelegramUser.AddAsync(queueUserRelation);
        await _dataContext.SaveChangesAsync();
    }

    public async Task LeaveAsync(Queue queue, TelegramUser user)
    {
        queue.Users.Remove(user);
        _dataContext.Queues.Update(queue);
        await _dataContext.SaveChangesAsync();
    }
}