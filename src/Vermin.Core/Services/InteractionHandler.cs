using Discord.Interactions;

namespace Vermin.Services;

public class InteractionHandler(
        ILogger<InteractionHandler> logger) : InteractionModuleBase<SocketInteractionContext>
{
    private readonly ILogger<InteractionHandler> _logger = logger;
}