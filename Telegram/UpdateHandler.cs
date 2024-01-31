using Newtonsoft.Json;
using QuqueBot.Telegram.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace QuqueBot.Telegram;

public class UpdateHandler : IUpdateHandler
{
    private readonly IEnumerable<ICommandHandler> _commandHandlers;
    private readonly IEnumerable<ICallbackHandler> _callbackHandlers;
    
    public UpdateHandler(IEnumerable<ICommandHandler> commandHandlers, IEnumerable<ICallbackHandler> callbackHandlers)
    {
        _commandHandlers = commandHandlers;
        _callbackHandlers = callbackHandlers;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(JsonConvert.SerializeObject(update, Formatting.Indented));

        var callbackData = update.CallbackQuery?.Data ?? string.Empty;
        foreach (var callbackHandler in _callbackHandlers.Where(handler => handler.Callbacks.ContainsKey(callbackData)))
        {
            await callbackHandler.Callbacks[callbackData].Invoke(update.CallbackQuery!);
            return;
        }

        var command = GetCommand(update.Message) ?? string.Empty;
        var commandParams = update.Message!.Text?
            .Split(" ")
            .Where(x => x != command && x != string.Empty)
            .ToList() ?? new List<string>();
        foreach (var handler in _commandHandlers.Where(handler => handler.Commands.ContainsKey(command)))
        {
            await handler.Commands[command].Invoke(update.Message!, commandParams);
            return;
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient telegramBotClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        
        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
    
    private static string? GetCommand(Message? message)
    {
        if (message?.Entities is null || message.Text is null)
        {
            return null;
        }
        return string.Join("", message.Entities
            .Where(x => x is { Type: MessageEntityType.BotCommand, Offset: 0 })
            .Select(x => message.Text[..x.Length]));
    }
}