using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Vermin.Models;

namespace Vermin.Services;

public class BotStartupService(
        ILogger<BotStartupService> logger,
        DiscordSocketClient socketClient,
        InteractionService interactionService,
        IOptions<TokenOptions> tokenOptions) : BackgroundService
{
    private readonly ILogger<BotStartupService> _logger = logger;
    private readonly DiscordSocketClient _socketClient = socketClient;
    private readonly InteractionService _interactionService = interactionService;
    private readonly TokenOptions _tokenOptions = tokenOptions.Value;

    protected override Task ExecuteAsync(
            CancellationToken stoppingToken)
    {
        _socketClient.Log += Log;
        _interactionService.Log += Log;

        return _socketClient.LoginAsync(
                tokenType: TokenType.Bot,
                token: _tokenOptions.DiscordBotToken,
                validateToken: true)
            .ContinueWith(_ =>
                    _socketClient.StartAsync(),
                    stoppingToken);
    }

    public override Task StopAsync(
            CancellationToken cancellationToken)
    {
        if (ExecuteTask is null)
            return Task.CompletedTask;

        base.StopAsync(cancellationToken);
        return _socketClient.StopAsync();
    }


    private Task Log(
            LogMessage message)
    {
        var level = message.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            _ => LogLevel.Debug
        };

        _logger.Log(
                logLevel: level,
                eventId: default,
                state: message,
                exception: message.Exception,
                formatter: (state, exception) => $"{state.Message} - {exception}");

        return Task.CompletedTask;
    }
}