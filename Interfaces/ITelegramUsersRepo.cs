using QuqueBot.Models;
using Telegram.Bot.Types;

namespace QuqueBot.Interfaces;

public interface ITelegramUsersRepo
{
    public Task<TelegramUser?> GetAsync(long id);

    public Task<TelegramUser> GetOrCreateAsync(User user);

    public Task<IEnumerable<TelegramUser>> GetAllAsync();

    public Task<long> CreateAsync(TelegramUser user);

    public Task UpdateAsync(TelegramUser user);

    public Task DeleteAsync(long id);
}