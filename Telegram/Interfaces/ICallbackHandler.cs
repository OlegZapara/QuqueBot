using Telegram.Bot.Types;

namespace QuqueBot.Telegram.Interfaces;

public interface ICallbackHandler
{
    public Dictionary<string, Func<CallbackQuery, Task>> Callbacks { get; set; }
}