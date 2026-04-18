using Discord;
using Discord.Interactions;

namespace Vermin.Services;

public class InteractionHandler(
        ILogger<InteractionHandler> logger) : InteractionModuleBase<SocketInteractionContext>
{
    private readonly ILogger<InteractionHandler> _logger = logger;

    //TODO:CreateIssueForum, SetupIssueForum, SyncIssueForum, GetTags, CreateTags, CreateDic

    // [SlashCommand("testforum", "forum")]
    // public async Task SetupForumAsync([ChannelTypes(ChannelType.Forum)] IForumChannel channel)
    // {
    // }


}