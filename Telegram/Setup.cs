using QuqueBot.Telegram.Callbacks;
using QuqueBot.Telegram.Commands;
using QuqueBot.Telegram.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace QuqueBot.Telegram;

public static class Setup
{
    public static void AddTelegramBotClient(this IServiceCollection services, string? telegramApiKey)
    {
        if (telegramApiKey is null)
        {
            throw new ArgumentNullException(telegramApiKey, "Telegram api key is required");
        }
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramApiKey));
        services.AddScoped<ICommandHandler, QueuesHandler>();
        services.AddScoped<ICommandHandler, RatingHandler>();
        services.AddScoped<ICommandHandler, TasksHandler>();
        services.AddScoped<ICommandHandler, TimetableHandler>();
        services.AddScoped<ICallbackHandler, QueuesCallbackHandler>();
        services.AddScoped<IUpdateHandler, UpdateHandler>();
    }

    public static void UseTelegramBotClient(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var telegramBotClient = scope.ServiceProvider.GetService<ITelegramBotClient>();
        var updateHandler = scope.ServiceProvider.GetService<IUpdateHandler>();
        if (telegramBotClient is null || updateHandler is null)
        {
            throw new ArgumentNullException(nameof(telegramBotClient), "Telegram bot client is null");
        }

        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };
        telegramBotClient.StartReceiving(updateHandler: updateHandler, receiverOptions: receiverOptions);
    }
}