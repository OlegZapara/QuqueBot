using QuqueBot.Telegram.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace QuqueBot.Telegram.Commands;

public class TimetableHandler : ICommandHandler
{
    public Dictionary<string, Func<Message, List<string>, Task>> Commands { get; set; }
    
    private readonly ITelegramBotClient _telegramBotClient;
    
    public TimetableHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
        Commands = new Dictionary<string, Func<Message, List<string>, Task>>()
        {
            { "/today", ViewTodayTimetableAsync },
            { "/week",  ViewWeekTimetableAsync },
            { "/now", ViewCurrentAsync }
        };
    }
    
    private static async Task ViewTodayTimetableAsync(Message msg, List<string> commandParams)
    {
        
    }

    private static async Task ViewWeekTimetableAsync(Message msg, List<string> commandParams)
    {
        
    }

    private static async Task ViewCurrentAsync(Message msg, List<string> commandParams)
    {
        
    }
}