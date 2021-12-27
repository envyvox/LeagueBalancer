using System.Threading.Tasks;

namespace LeagueBalancer.Services.Discord.Client
{
    public interface IDiscordClientService
    {
        Task Start();
    }
}