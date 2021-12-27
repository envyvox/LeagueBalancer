using System;

namespace LeagueBalancer.Data
{
    public enum EmoteType : byte
    {
        Arrow,
        Unranked,
        Iron,
        Bronze,
        Silver,
        Gold,
        Platinum,
        Diamond,
        Master,
        Grandmaster,
        Challenger
    }

    public static class EmoteHelper
    {
        public static string Display(this EmoteType emote) => emote switch
        {
            EmoteType.Arrow => "<:Arrow:874170861450575945>",
            EmoteType.Unranked => "<:Unranked:924914240232497223>",
            EmoteType.Iron => "<:Iron:924914239896965161>",
            EmoteType.Bronze => "<:Bronze:924914718647406632>",
            EmoteType.Silver => "<:Silver:924914239938920510>",
            EmoteType.Gold => "<:Gold:924914240156999710>",
            EmoteType.Platinum => "<:Platinum:924914240161185803>",
            EmoteType.Diamond => "<:Diamond:924914240198934548>",
            EmoteType.Master => "<:Master:924914240186384404>",
            EmoteType.Grandmaster => "<:Grandmaster:924914240018591765>",
            EmoteType.Challenger => "<:Challenger:924914240203128843>",
            _ => throw new ArgumentOutOfRangeException(nameof(emote), emote, null)
        };
    }
}