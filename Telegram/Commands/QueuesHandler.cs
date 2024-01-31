using QuqueBot.Interfaces;
using QuqueBot.Models;
using QuqueBot.Telegram.Enums;
using QuqueBot.Telegram.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace QuqueBot.Telegram.Commands;

public class QueuesHandler : ICommandHandler
{
    public Dictionary<string, Func<Message, List<string>, Task>> Commands { get; set; }

    private readonly ITelegramBotClient _telegramBotClient;

    private readonly IQueuesRepo _queuesRepo;

    public QueuesHandler(ITelegramBotClient telegramBotClient, IQueuesRepo queuesRepo)
    {
        _telegramBotClient = telegramBotClient;
        _queuesRepo = queuesRepo;
        Commands = new Dictionary<string, Func<Message, List<string>, Task>>()
        {
            { "/queue", CreateAsync },
            { "/quque", CreateAsync }
        };
    }

    private async Task CreateAsync(Message msg, List<string> commandParams)
    {
        var subcommand = commandParams.FirstOrDefault();
        if (subcommand is null)
        {
            await _telegramBotClient.SendTextMessageAsync(msg.Chat.Id, "Треба вказати назву");
            return;
        }
        switch (subcommand)
        {
            case "delete":
                await DeleteAsync(msg, commandParams);
                return;
            case "edit":
                await UpdateAsync(msg, commandParams);
                return;
        }

        var keyboardMarkup = new InlineKeyboardMarkup(new []
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Join", callbackData: QueueCallback.JoinQueue),
                InlineKeyboardButton.WithCallbackData(text: "Leave", callbackData: QueueCallback.LeaveQueue),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Delete", callbackData: QueueCallback.DeleteQueue),
                InlineKeyboardButton.WithCallbackData(text: "Notify", callbackData: QueueCallback.NotifyQueue),
                InlineKeyboardButton.WithCallbackData(text: "Shuffle", callbackData: QueueCallback.ShuffleQueue),
            },
        });
        var title = string.Join(" ", commandParams);
        const string leftDecoration = ">>> ";
        const string rightDecoration = " <<<";
        var createdMessage = await _telegramBotClient.SendTextMessageAsync(
            msg.Chat.Id,
            leftDecoration + title.ToUpper() + rightDecoration,
            replyMarkup: keyboardMarkup, 
            entities: new MessageEntity[]
            {
                new()
                {
                    Type = MessageEntityType.Bold,
                    Offset = leftDecoration.Length,
                    Length = title.Length
                }
            });
        var newQueue = new Queue()
        {
            Name = title,
            IsDeleted = false,
            IsMixed = false,
            ChatId = createdMessage.Chat.Id,
            MessageId = createdMessage.MessageId,
            Users = new List<TelegramUser>()
        };
        await _queuesRepo.CreateAsync(newQueue);
    }

    private async Task DeleteAsync(Message msg, List<string> commandParams)
    {
        await _telegramBotClient.SendTextMessageAsync(msg.Chat.Id, "Видалення");
    }

    private async Task UpdateAsync(Message msg, List<string> commandParams)
    {
        await _telegramBotClient.SendTextMessageAsync(msg.Chat.Id, "Оновлення");
    }
}