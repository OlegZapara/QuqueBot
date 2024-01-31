using System.Text;
using QuqueBot.Interfaces;
using QuqueBot.Models;
using QuqueBot.Telegram.Enums;
using QuqueBot.Telegram.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace QuqueBot.Telegram.Callbacks;

public class QueuesCallbackHandler : ICallbackHandler
{
    public Dictionary<string, Func<CallbackQuery, Task>> Callbacks { get; set; }

    private readonly ITelegramBotClient _telegramBotClient;

    private readonly IQueuesRepo _queuesRepo;

    private readonly ITelegramUsersRepo _telegramUsersRepo;

    public QueuesCallbackHandler(ITelegramBotClient telegramBotClient, ITelegramUsersRepo telegramUsersRepo, IQueuesRepo queuesRepo)
    {
        _telegramBotClient = telegramBotClient;
        _telegramUsersRepo = telegramUsersRepo;
        _queuesRepo = queuesRepo;
        Callbacks = new Dictionary<string, Func<CallbackQuery, Task>>()
        {
            { QueueCallback.JoinQueue, JoinQueue },
            { QueueCallback.LeaveQueue, LeaveQueue },
            { QueueCallback.NotifyQueue, NotifyQueue },
            { QueueCallback.ShuffleQueue, ShuffleQueue },
            { QueueCallback.DeleteQueue, DeleteQueue }
        };
    }

    private async Task JoinQueue(CallbackQuery query)
    {
        var existingUser = await _telegramUsersRepo.GetOrCreateAsync(query.From);
        var queue = await _queuesRepo.GetByMessageIdAsync(query.Message!.MessageId);
        if (queue is null || queue.IsDeleted)
        {
            return;
        }
        if (existingUser.Queues.Find(x => x.Id == queue.Id) is not null)
        {
            return;
        }

        await _queuesRepo.JoinAsync(queue, existingUser);
        // TODO: maybe refactor message sending
        await _telegramBotClient.EditMessageTextAsync(
            chatId: queue.ChatId,
            messageId: (int)queue.MessageId,
            text: GetFormattedQueueMessageText(queue),
            replyMarkup: GetQueueInlineMarkup(queue.IsMixed),
            entities: new MessageEntity[]
        {
            new()
            {
                Type = MessageEntityType.Bold,
                Offset = 4,
                Length = queue.Name!.Length
            }
        });
    }

    private async Task LeaveQueue(CallbackQuery query)
    {
        var existingUser = await _telegramUsersRepo.GetOrCreateAsync(query.From);
        var queue = await _queuesRepo.GetByMessageIdAsync(query.Message!.MessageId);
        if (queue is null || queue.IsDeleted)
        {
            return;
        }
        if (existingUser.Queues.Find(x => x.Id == queue.Id) is null)
        {
            return;
        }

        await _queuesRepo.LeaveAsync(queue, existingUser);
        await _telegramBotClient.EditMessageTextAsync(
            chatId: queue.ChatId,
            messageId: (int)queue.MessageId,
            text: GetFormattedQueueMessageText(queue),
            replyMarkup: GetQueueInlineMarkup(queue.IsMixed),
            entities: new MessageEntity[]
            {
                new()
                {
                    Type = MessageEntityType.Bold,
                    Offset = 4,
                    Length = queue.Name!.Length
                }
            });
    }
    
    private async Task NotifyQueue(CallbackQuery query)
    {
        var existingUser = await _telegramUsersRepo.GetOrCreateAsync(query.From);
        var queue = await _queuesRepo.GetByMessageIdAsync(query.Message!.MessageId);
        if (queue is null)
        {
            return;
        }

        var firstInQueue = queue.Users.FirstOrDefault();
        if (firstInQueue is null)
        {
            return;
        }

        await _telegramBotClient.SendTextMessageAsync(
            query.Message.Chat.Id,
            $"\u2757\ufe0f {firstInQueue.FirstName} (@{firstInQueue.Username}), твоя черга відповідати у {queue.Name}!\n({RandomAdverb()} зазначає {existingUser.FirstName})", 
            replyToMessageId: query.Message.MessageId);
    }

    private string RandomAdverb()
    {
        var possibleAnswers = new List<string>() { "\ud83d\ude4a тихо", "\ud83d\ude21 із злістю", "\ud83d\ude42 лагідно", "\ud83d\ude0c спокійно", "\ud83e\udee1 з повагою", "\ud83e\udd17 відкрито", "\ud83d\ude09 ласкаво", "\ud83d\ude22 сумно", "\ud83d\ude01 доброзичливо", "\ud83e\udd2c з обуренням", "\ud83d\ude12 вкотре"};
        var random = new Random();
        return possibleAnswers[random.Next(possibleAnswers.Count)];
    }
    
    private async Task ShuffleQueue(CallbackQuery query)
    {
        var random = new Random();
        var existingUser = await _telegramUsersRepo.GetOrCreateAsync(query.From);
        var queue = await _queuesRepo.GetByMessageIdAsync(query.Message!.MessageId);
        if (queue is null || queue.IsDeleted || !existingUser.IsAdmin)
        {
            return;
        }
        // TODO: every user entity should have position in queue
        queue.Users = queue.Users.OrderBy(x => random.Next()).ToList();
        queue.IsMixed = true;
        await _queuesRepo.UpdateAsync(queue);
        await _telegramBotClient.EditMessageTextAsync(
            chatId: queue.ChatId,
            messageId: (int)queue.MessageId,
            text: GetFormattedQueueMessageText(queue),
            replyMarkup: GetQueueInlineMarkup(queue.IsMixed),
            entities: new MessageEntity[]
            {
                new()
                {
                    Type = MessageEntityType.Bold,
                    Offset = 4,
                    Length = queue.Name!.Length
                }
            });
    }
    
    private async Task DeleteQueue(CallbackQuery query)
    {
        var existingUser = await _telegramUsersRepo.GetOrCreateAsync(query.From);
        var queue = await _queuesRepo.GetByMessageIdAsync(query.Message!.MessageId);
        if (queue is null || queue.IsDeleted || !existingUser.IsAdmin)
        {
            return;
        }

        queue.IsDeleted = true;
        await _queuesRepo.UpdateAsync(queue);
        await _telegramBotClient.DeleteMessageAsync(queue.ChatId, (int)queue.MessageId);
    }

    private string GetFormattedQueueMessageText(Queue queue)
    {
        var stringBuilder = new StringBuilder($">>> {queue.Name} <<<\n\n");
        var userIndex = 1;
        foreach (var user in queue.Users)
        {
            stringBuilder.AppendLine($"ID: {userIndex++} - {user.FirstName} (@{user.Username})");
        }
        return stringBuilder.ToString();
    }

    private InlineKeyboardMarkup GetQueueInlineMarkup(bool isMixed)
    {
        var firstRow = new[]
        {
            InlineKeyboardButton.WithCallbackData(text: "Join", callbackData: QueueCallback.JoinQueue),
            InlineKeyboardButton.WithCallbackData(text: "Leave", callbackData: QueueCallback.LeaveQueue),
        };
        var secondRow = new[]
        {
            InlineKeyboardButton.WithCallbackData(text: "Delete", callbackData: QueueCallback.DeleteQueue),
            InlineKeyboardButton.WithCallbackData(text: "Notify", callbackData: QueueCallback.NotifyQueue),
        };
        if (!isMixed)
        {
            secondRow = secondRow
                .Append(
                    InlineKeyboardButton.WithCallbackData(text: "Shuffle", callbackData: QueueCallback.ShuffleQueue))
                .ToArray();
        }
        return new InlineKeyboardMarkup(new []
        {
            firstRow,
            secondRow
        });
    }
}