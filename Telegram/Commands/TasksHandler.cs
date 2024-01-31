using QuqueBot.Telegram.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace QuqueBot.Telegram.Commands;

public class TasksHandler : ICommandHandler
{
    public Dictionary<string, Func<Message, List<string>, Task>> Commands { get; set; }
    
    private readonly ITelegramBotClient _telegramBotClient;
    
    public TasksHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
        Commands = new Dictionary<string, Func<Message, List<string>, Task>>()
        {
            { "/task", CreateTaskAsync },
            { "/edit",  EditTaskAsync },
            { "/due", ViewCurrentTasksAsync }
        };
    }
    
    private static async Task CreateTaskAsync(Message msg, List<string> commandParams)
    {
        
    }

    private static async Task EditTaskAsync(Message msg, List<string> commandParams)
    {
        
    }

    private static async Task ViewCurrentTasksAsync(Message msg, List<string> commandParams)
    {
        
    }
}