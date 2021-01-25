using System.Collections.Generic;
using FantasyBaseball.CommonModels.Enums;
using FantasyBaseball.CommonModels.Player;
using FantasyBaseball.CommonModels.Stats;
using FantasyBaseball.PlayerMergeService.Services;
using Xunit;

namespace FantasyBaseball.PlayerMergeService.UnitTests.Services
{
    public class MergeServiceTest
    {
        [Fact] public void MergePlayersNull() => Assert.Empty(new MergeService().MergePlayers(null, null, null));

        [Fact] public void MergePlayerValid()
        {
            var existing = new List<BaseballPlayer>
            {
                BuildPlayer(1,  1, PlayerType.B, true),
                BuildPlayer(1,  2, PlayerType.P, true),
                BuildPlayer(2,  3, PlayerType.B, true),
                BuildPlayer(3,  4, PlayerType.P, true),
                BuildPlayer(4,  5, PlayerType.B, true),
                BuildPlayer(5,  6, PlayerType.P, true),
                BuildPlayer(8, 13, PlayerType.B, true),
                BuildPlayer(9, 14, PlayerType.P, true),
            };
            var batters = new List<BaseballPlayer>
            {
                BuildPlayer(1,  7, PlayerType.B, false),
                BuildPlayer(2,  8, PlayerType.B, false),
                BuildPlayer(6,  9, PlayerType.B, false),
                BuildPlayer(9, 15, PlayerType.B, false),
            };
            var pitchers = new List<BaseballPlayer>
            {
                BuildPlayer(1, 10, PlayerType.P, false),
                BuildPlayer(3, 11, PlayerType.P, false),
                BuildPlayer(7, 12, PlayerType.P, false),
                BuildPlayer(8, 16, PlayerType.P, false),
            };
            var results = new MergeService().MergePlayers(existing, batters, pitchers);
            Assert.Equal(12, results.Count);
            ValidatePlayer(existing[0],  batters[0], results[ 0]);
            ValidatePlayer(existing[2],  batters[1], results[ 1]);
            ValidatePlayer(null,         batters[2], results[ 2]);
            ValidatePlayer(null,         batters[3], results[ 3]);
            ValidatePlayer(existing[1], pitchers[0], results[ 4]);
            ValidatePlayer(existing[3], pitchers[1], results[ 5]);
            ValidatePlayer(null,        pitchers[2], results[ 6]);
            ValidatePlayer(null,        pitchers[3], results[ 7]);
            ValidatePlayer(existing[4], null,        results[ 8]);
            ValidatePlayer(existing[5], null,        results[ 9]);
            ValidatePlayer(existing[6], null,        results[10]);
            ValidatePlayer(existing[7], null,        results[11]);
        }

        private static BaseballPlayer BuildPlayer(int id, int value, PlayerType type, bool existing) =>
            new BaseballPlayer 
            {
                PlayerInfo = new PlayerInfo
                {
                    Id = id,
                    FirstName = $"First-{value}",
                    LastName = $"Last-{value}",
                    Age = value,
                    Type = type,
                    Positions = $"Pos-{value}",
                    Team = $"Team-{value}",
                    Status = existing ? PlayerStatus.XX : PlayerStatus.DL,
                },
                LeagueInfo = new LeagueInfo { League1 = existing ? LeagueStatus.R : LeagueStatus.S },
                DraftInfo = new DraftInfo { DraftRank = existing ? value : value * -1 },
                BhqScores = new BhqScores { MayberryMethod = existing ? value * -1 : value }, 
                YearToDateBattingStats = new BattingStats { AtBats = existing && PlayerType.B == type ? value : 0 },
                YearToDatePitchingStats = new PitchingStats { InningsPitched = existing && PlayerType.P == type ? value : 0 },
                ProjectedBattingStats = new BattingStats { AtBats = existing && PlayerType.B == type ? value * 10 : 0 },
                ProjectedPitchingStats = new PitchingStats { InningsPitched = existing && PlayerType.P == type ? value * 10 : 0 },
            };

        private static void ValidatePlayer(BaseballPlayer existing, BaseballPlayer data, BaseballPlayer results)
        {
            Assert.Equal((existing ?? data).PlayerInfo.Id, results.PlayerInfo.Id);
            Assert.Equal((existing ?? data).PlayerInfo.FirstName, results.PlayerInfo.FirstName);
            Assert.Equal((existing ?? data).PlayerInfo.LastName, results.PlayerInfo.LastName);
            Assert.Equal((data ?? existing).PlayerInfo.Age, results.PlayerInfo.Age);
            Assert.Equal((existing ?? data).PlayerInfo.Type, results.PlayerInfo.Type);
            Assert.Equal((existing ?? data).PlayerInfo.Positions, results.PlayerInfo.Positions);
            Assert.Equal((data ?? existing).PlayerInfo.Team, results.PlayerInfo.Team);
            Assert.Equal((existing ?? data).PlayerInfo.Status, results.PlayerInfo.Status);
            Assert.Equal((existing ?? new BaseballPlayer()).LeagueInfo.League1, results.LeagueInfo.League1);
            Assert.Equal((existing ?? new BaseballPlayer()).DraftInfo.DraftRank, results.DraftInfo.DraftRank);
            Assert.Equal((data ?? new BaseballPlayer()).BhqScores.MayberryMethod, results.BhqScores.MayberryMethod);
            Assert.Equal((data ?? new BaseballPlayer()).YearToDateBattingStats.AtBats, results.YearToDateBattingStats.AtBats);
            Assert.Equal((data ?? new BaseballPlayer()).YearToDatePitchingStats.InningsPitched, results.YearToDatePitchingStats.InningsPitched);
            Assert.Equal((data ?? new BaseballPlayer()).ProjectedBattingStats.AtBats, results.ProjectedBattingStats.AtBats);
            Assert.Equal((data ?? new BaseballPlayer()).ProjectedPitchingStats.InningsPitched, results.ProjectedPitchingStats.InningsPitched);
            Assert.Equal((data ?? new BaseballPlayer()).YearToDateBattingStats.AtBats + (data ?? new BaseballPlayer()).ProjectedBattingStats.AtBats, 
                results.CombinedBattingStats.AtBats);
            Assert.Equal((data ?? new BaseballPlayer()).YearToDatePitchingStats.InningsPitched + (data ?? new BaseballPlayer()).ProjectedPitchingStats.InningsPitched, 
                results.CombinedPitchingStats.InningsPitched);
        }
    }
}