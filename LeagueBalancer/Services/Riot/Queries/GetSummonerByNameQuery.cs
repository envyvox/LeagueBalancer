using System;
using System.Threading;
using System.Threading.Tasks;
using LeagueBalancer.Data;
using MediatR;
using Microsoft.Extensions.Options;
using RiotNet;
using RiotNet.Models;

namespace LeagueBalancer.Services.Riot.Queries
{
    public record GetSummonerByNameQuery(RegionType Region, string SummonerName) : IRequest<Summoner>;

    public class GetSummonerByNameHandler : IRequestHandler<GetSummonerByNameQuery, Summoner>
    {
        private readonly LbOptions _lbOptions;

        public GetSummonerByNameHandler(IOptions<LbOptions> options)
        {
            _lbOptions = options.Value;
        }

        public async Task<Summoner> Handle(GetSummonerByNameQuery request, CancellationToken ct)
        {
            var riotClient = new RiotClient(new RiotClientSettings
            {
                ApiKey = _lbOptions.RiotToken
            });

            var summoner = await riotClient.GetSummonerBySummonerNameAsync(
                request.SummonerName, request.Region.ToPlatformId());

            if (summoner is null)
            {
                throw new Exception(
                    $"summoner with name **{request.SummonerName}** was not found on region **{request.Region.ToString()}**. Double check your spelling or region.");
            }

            return summoner;
        }
    }
}