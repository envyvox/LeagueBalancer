using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace LeagueBalancer.Services.Discord.Client
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(
            DiscordSocketClient client,
            InteractionService commands,
            IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.InteractionCreated += HandleInteraction;
            _commands.SlashCommandExecuted += SlashCommandExecuted;
            _commands.ContextCommandExecuted += ContextCommandExecuted;
            _commands.ComponentCommandExecuted += ComponentCommandExecuted;
        }

        private static Task ComponentCommandExecuted(ComponentCommandInfo componentCommandInfo,
            IInteractionContext interactionContext, IResult result)
        {
            if (!result.IsSuccess)
            {
                interactionContext.Interaction.FollowupAsync(
                    $"{interactionContext.User.Mention}, {result.ErrorReason}.",
                    ephemeral: true);
            }

            return Task.CompletedTask;
        }

        private static Task ContextCommandExecuted(ContextCommandInfo contextCommandInfo,
            IInteractionContext interactionContext, IResult result)
        {
            if (!result.IsSuccess)
            {
                interactionContext.Interaction.FollowupAsync(
                    $"{interactionContext.User.Mention}, {result.ErrorReason}.",
                    ephemeral: true);
            }

            return Task.CompletedTask;
        }

        private static Task SlashCommandExecuted(SlashCommandInfo slashCommandInfo,
            IInteractionContext interactionContext, IResult result)
        {
            if (!result.IsSuccess)
            {
                interactionContext.Interaction.FollowupAsync(
                    $"{interactionContext.User.Mention}, {result.ErrorReason}.",
                    ephemeral: true);
            }

            return Task.CompletedTask;
        }

        private async Task HandleInteraction(SocketInteraction socketInteraction)
        {
            try
            {
                // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var ctx = new SocketInteractionContext(_client, socketInteraction);
                await _commands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                // If a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if (socketInteraction.Type == InteractionType.ApplicationCommand)
                    await socketInteraction.GetOriginalResponseAsync()
                        .ContinueWith(async msg => await msg.Result.DeleteAsync());
            }
        }
    }
}