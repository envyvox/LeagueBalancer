using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using LeagueBalancer.Data;
using LeagueBalancer.Services.Riot.Extensions;
using LeagueBalancer.Services.Riot.Queries;
using MediatR;
using RiotNet.Models;

namespace LeagueBalancer.Services.Discord.Commands
{
    public class BalanceCommand : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public BalanceCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SlashCommand("balance", "Balance a custom lobby")]
        public async Task BalanceCommandTask(
            [Summary("region", "Region (server) where the lobby is located")] RegionType region,
            [Summary("names", "Summoner names. Use , as a divider")] string input)
        {
            await Context.Interaction.DeferAsync();

            var summonerNames = input.Split(",");

            if (summonerNames.Length < 2)
            {
                throw new Exception(
                    "you must enter **at least** 2 summoner names");
            }
            
            var summoners = new Dictionary<Summoner, long>();

            foreach (var summonerName in summonerNames)
            {
                var summoner = await _mediator.Send(new GetSummonerByNameQuery(region, summonerName));
                var entries = await _mediator.Send(new GetLeagueEntriesBySummonerIdQuery(region, summoner.Id));
                var soloQ = entries.SingleOrDefault(x => x.QueueType.Contains("SOLO"));
                var points = soloQ is null ? 0 : (soloQ.Tier + soloQ.Rank.ToArabic()).ToPoints() + soloQ.LeaguePoints;

                summoners.Add(summoner, points);
            }

            var teams = BalanceTeams(summoners);

            var embed = new EmbedBuilder();

            var counter = 1;
            foreach (var team in teams)
            {
                embed.AddField($"`{team.Sum(x => x.Value)}` Team {counter}",
                    team.Aggregate(string.Empty, (s, v) =>
                        s + $"{EmoteType.Arrow.Display()} {v.Value.ToRankEmote()} `{v.Value}` {v.Key.Name}\n"));
                counter++;
            }

            await FollowupAsync(embed: embed.Build());
        }

        private static IEnumerable<List<KeyValuePair<Summoner, long>>> BalanceTeams(
            Dictionary<Summoner, long> summoners)
        {
            var summonersPool = summoners
                .OrderBy(x => x.Value)
                .ToList();

            var totalPoints = summonersPool.Sum(summoner => summoner.Value);
            var halfOfTotalPoints = totalPoints / 2f;

            var team1 = new List<KeyValuePair<Summoner, long>>();
            var team2 = new List<KeyValuePair<Summoner, long>>();

            long team1Points = 0, team2Points = 0;
            var team2MembersCounter = 0;

            for (var i = summoners.Count - 1; i >= 0; i--)
            {
                if (team1Points <= team2Points || team2Points >= halfOfTotalPoints || team2MembersCounter == 5)
                {
                    team1.Add(summonersPool[i]);
                    team1Points += summonersPool[i].Value;
                }
                else
                {
                    team2.Add(summonersPool[i]);
                    team2Points += summonersPool[i].Value;
                    team2MembersCounter++;
                }
            }

            return new[] {team1, team2};
        }
    }
}