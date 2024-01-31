using QuqueBot.Models;

namespace QuqueBot.Interfaces;

public interface IQueuesRepo
{
    public Task<Queue?> GetAsync(long id);

    public Task<Queue?> GetByMessageIdAsync(long id);

    public Task<IEnumerable<Queue>> GetAllAsync();

    public Task ShuffleAsync(Queue queue);

    public Task<long> CreateAsync(Queue queue);

    public Task UpdateAsync(Queue queue);

    public Task DeleteAsync(long id);

    public Task JoinAsync(Queue queue, TelegramUser user);

    public Task LeaveAsync(Queue queue, TelegramUser user);
}