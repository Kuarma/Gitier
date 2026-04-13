using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Vermin.Services;

public class InteractionStartupService(
        InteractionService service,
        IServiceProvider serviceProvider,
        DiscordSocketClient socketClient,
        ILogger<InteractionStartupService> logger) : BackgroundService
{
    private readonly InteractionService _interactionService = service;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly DiscordSocketClient _socketClient = socketClient;
    private readonly ILogger<InteractionStartupService> _logger = logger;

    protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
    {
        await _interactionService.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: _serviceProvider);

        _socketClient.InteractionCreated += HandleInteraction;
        _interactionService.InteractionExecuted += HandleInteractionExecuted;
        _socketClient.Ready += RegisterCommands;
    }

    //TODO: Handle integration stuff
    private async Task HandleInteraction(
            SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(
                    client: _socketClient,
                    interaction: interaction);

            await _interactionService.ExecuteCommandAsync(
                    context: context,
                    services: _serviceProvider);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                    exception: exception,
                    message: "Interaction execution failed");

            if (interaction.HasResponded)
                return;

            await interaction.RespondAsync(
                    text: "Error processing command",
                    ephemeral: true);
        }
    }

    private Task HandleInteractionExecuted(
        ICommandInfo command,
        IInteractionContext context,
        IResult result)
    {
        return Task.CompletedTask;
    }

    private async Task RegisterCommands()
    {
        await _interactionService.RegisterCommandsGloballyAsync(
                deleteMissing: true);
    }
}