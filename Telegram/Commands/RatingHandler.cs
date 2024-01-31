using QuqueBot.Telegram.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace QuqueBot.Telegram.Commands;

public class RatingHandler : ICommandHandler
{
    public Dictionary<string, Func<Message, List<string>, Task>> Commands { get; set; }
    
    private readonly ITelegramBotClient _telegramBotClient;
    
    public RatingHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
        Commands = new Dictionary<string, Func<Message, List<string>, Task>>()
        {
            { "/stats", ViewRatingAsync },
            { "/me",  ViewUserRatingAsync },
            { "/increase", IncreaseUserRatingAsync },
            { "/decrease", DecreaseUserRatingAsync }
        };
    }

    private async Task ViewRatingAsync(Message msg, List<string> commandParams)
    {
        
    }

    private async Task ViewUserRatingAsync(Message msg, List<string> commandParams)
    {
        
    }

    private async Task IncreaseUserRatingAsync(Message msg, List<string> commandParams)
    {
        
    }

    private async Task DecreaseUserRatingAsync(Message msg, List<string> commandParams)
    {
        
    }
    
    
}