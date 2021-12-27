using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LeagueBalancer.Data;
using MediatR;
using Microsoft.Extensions.Options;
using RiotNet;
using RiotNet.Models;

namespace LeagueBalancer.Services.Riot.Queries
{
    public record GetLeagueEntriesBySummonerIdQuery(RegionType Region, string SummonerId) : IRequest<List<LeagueEntry>>;

    public class GetLeagueEntriesBySummonerIdHandler
        : IRequestHandler<GetLeagueEntriesBySummonerIdQuery, List<LeagueEntry>>
    {
        private readonly LbOptions _lbOptions;

        public GetLeagueEntriesBySummonerIdHandler(IOptions<LbOptions> options)
        {
            _lbOptions = options.Value;
        }

        public async Task<List<LeagueEntry>> Handle(GetLeagueEntriesBySummonerIdQuery request, CancellationToken ct)
        {
            var riotClient = new RiotClient(new RiotClientSettings
            {
                ApiKey = _lbOptions.RiotToken
            });

            var entries = await riotClient.GetLeagueEntriesBySummonerIdAsync(
                request.SummonerId, request.Region.ToPlatformId());

            if (entries is null)
            {
                throw new Exception(
                    "summoner doesnt have **any** ranked info");
            }

            return entries;
        }
    }
}