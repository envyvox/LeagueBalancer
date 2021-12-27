using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using LeagueBalancer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LeagueBalancer.Services.Discord.Client
{
    public class DiscordClientService : IDiscordClientService
    {
        private readonly LbOptions _lbOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly InteractionService _interactionService;
        private readonly IWebHostEnvironment _env;

        public DiscordClientService(
            IOptions<LbOptions> options,
            IServiceProvider serviceProvider,
            InteractionService interactionService,
            IWebHostEnvironment env)
        {
            _lbOptions = options.Value;
            _serviceProvider = serviceProvider;
            _interactionService = interactionService;
            _env = env;
        }

        public async Task Start()
        {
            var client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
            var commands = _serviceProvider.GetRequiredService<InteractionService>();

            client.Log += SocketClientOnLog;
            commands.Log += SocketClientOnLog;

            client.Ready += async () =>
            {
                if (_env.IsDevelopment())
                    await _interactionService.RegisterCommandsToGuildAsync(_lbOptions.DevGuildId);
                else
                    await _interactionService.RegisterCommandsGloballyAsync();

                await client.SetGameAsync($"/balance | {client.Guilds.Count} servers", type: ActivityType.Watching);
            };

            await _serviceProvider.GetRequiredService<CommandHandler>().InitializeAsync();

            await client.LoginAsync(TokenType.Bot, _lbOptions.DiscordToken);
            await client.StartAsync();
        }

        private static Task SocketClientOnLog(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}