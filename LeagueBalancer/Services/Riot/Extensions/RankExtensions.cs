using System;
using System.Collections.Generic;
using System.Linq;
using LeagueBalancer.Data;

namespace LeagueBalancer.Services.Riot.Extensions
{
    public static class RankExtensions
    {
        public static uint ToArabic(this string roman) => roman switch
        {
            "I" => 1,
            "II" => 2,
            "III" => 3,
            "IV" => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(roman), roman, null)
        };

        public static uint ToPoints(this string rank)
        {
            rank = rank.ToLower();
            int division;

            if (rank.Contains("iron"))
            {
                division = (int) char.GetNumericValue(rank[^1]);

                return division switch
                {
                    1 => 300,
                    2 => 200,
                    3 => 100,
                    4 => 0,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            if (rank.Contains("bronze"))
            {
                division = (int) char.GetNumericValue(rank[^1]);

                return division switch
                {
                    1 => 700,
                    2 => 600,
                    3 => 500,
                    4 => 400,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            if (rank.Contains("silver"))
            {
                division = (int) char.GetNumericValue(rank[^1]);

                return division switch
                {
                    1 => 1100,
                    2 => 1000,
                    3 => 900,
                    4 => 800,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            if (rank.Contains("gold"))
            {
                division = (int) char.GetNumericValue(rank[^1]);

                return division switch
                {
                    1 => 1500,
                    2 => 1400,
                    3 => 1300,
                    4 => 1200,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            if (rank.Contains("platinum"))
            {
                division = (int) char.GetNumericValue(rank[^1]);

                return division switch
                {
                    1 => 1900,
                    2 => 1800,
                    3 => 1700,
                    4 => 1600,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            if (rank.Contains("diamond"))
            {
                division = (int) char.GetNumericValue(rank[^1]);

                return division switch
                {
                    1 => 2300,
                    2 => 2200,
                    3 => 2100,
                    4 => 2000,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            if (rank.Contains("master"))
            {
                return (uint) (rank.Contains("grand") ? 2500 : 2400);
            }

            if (rank.Contains("challenger"))
            {
                return 2600;
            }

            return 0;
        }

        public static string ToRankEmote(this long points)
        {
            var dict = new Dictionary<long, string>
            {
                {0, EmoteType.Unranked.Display()},
                {1, EmoteType.Iron.Display()},
                {400, EmoteType.Bronze.Display()},
                {800, EmoteType.Silver.Display()},
                {1200, EmoteType.Gold.Display()},
                {1600, EmoteType.Platinum.Display()},
                {2000, EmoteType.Diamond.Display()},
                {2400, EmoteType.Master.Display()},
                {2500, EmoteType.Grandmaster.Display()},
                {2600, EmoteType.Challenger.Display()}
            };

            return dict[dict.Keys.Where(x => x <= points).Max()];
        }
    }
}