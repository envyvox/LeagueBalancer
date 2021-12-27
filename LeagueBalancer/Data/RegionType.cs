using System;

namespace LeagueBalancer.Data
{
    public enum RegionType : byte
    {
        Russia = 1,
        EuropeWest = 2,
        NorthAmerica = 3,
        EuropeNordicEast = 4,
        Korea = 5,
        Japan = 6,
        Oceania = 7,
        Brazil = 8,
        Turkey = 9
    }

    public static class RegionHelper
    {
        public static string ToPlatformId(this RegionType region) => region switch
        {
            RegionType.Russia => "RU",
            RegionType.EuropeWest => "EUW1",
            RegionType.NorthAmerica => "NA1",
            RegionType.EuropeNordicEast => "EUN1",
            RegionType.Korea => "KR",
            RegionType.Japan => "JP1",
            RegionType.Oceania => "OC1",
            RegionType.Brazil => "BR1",
            RegionType.Turkey => "TR1",
            _ => throw new ArgumentOutOfRangeException(nameof(region), region, null)
        };
    }
}