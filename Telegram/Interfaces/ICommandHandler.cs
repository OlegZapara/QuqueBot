using Telegram.Bot.Types;

namespace QuqueBot.Telegram.Interfaces;

public interface ICommandHandler
{
    public Dictionary<string, Func<Message, List<string>, Task>> Commands { get; set; }
}