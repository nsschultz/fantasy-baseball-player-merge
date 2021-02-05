using System;
using System.Collections.Generic;
using System.Linq;
using FantasyBaseball.Common.Models;
using FantasyBaseball.Common.Models.Builders;
using FantasyBaseball.PlayerMergeService.Models;

namespace FantasyBaseball.PlayerMergeService.Services
{
    /// <summary>The service for merging the new stats to the existing players.</summary>
    public class MergeService : IMergeService
    {
        /// <summary>Merges the new stats into the existing player list.</summary>
        /// <param name="existingPlayers">The existing collection of players.</param>
        /// <param name="batters">The new batting stats data</param>
        /// <param name="pitchers">The new pitching stats data.</param>
        /// <returns>The merged collection of players.</returns>
        public List<BaseballPlayer> MergePlayers(IEnumerable<BaseballPlayer> existingPlayers, 
                                                 IEnumerable<BaseballPlayer> batters, 
                                                 IEnumerable<BaseballPlayer> pitchers)
        {
            existingPlayers = existingPlayers ?? new List<BaseballPlayer>();
            batters = batters ?? new List<BaseballPlayer>();
            pitchers = pitchers  ?? new List<BaseballPlayer>();
            var existingDictionary = existingPlayers.GroupBy(p => BuildKey(p)).ToDictionary(g => g.Key, g => g.First());
            var newData = MergePlayers(existingDictionary, batters, pitchers);
            LogStats(existingPlayers, batters, pitchers, newData);
            return newData;
        }

        private static BhqPlayerKey BuildKey(BaseballPlayer player) => new BhqPlayerKey(player.BhqId, player.Type);

        private static BaseballPlayer FindAndRemovePlayer(Dictionary<BhqPlayerKey, BaseballPlayer> existingDictionary, BhqPlayerKey key)
        {
            if (!existingDictionary.ContainsKey(key)) return null;
            var existingPlayer = existingDictionary[key];
            existingDictionary.Remove(key);
            return existingPlayer;
        }

        private static void LogStats(IEnumerable<BaseballPlayer> existingPlayers, 
                                     IEnumerable<BaseballPlayer> batters,
                                     IEnumerable<BaseballPlayer> pitchers,
                                     IEnumerable<BaseballPlayer> mergedResults)
        {
            Console.WriteLine($"Recieved {existingPlayers.Count()} existing players");
            Console.WriteLine($"Recieved {batters.Count()} batters");
            Console.WriteLine($"Recieved {pitchers.Count()} pitchers");
            Console.WriteLine($"Merged Results: {mergedResults.Count()} players");
        }
        
        private static BaseballPlayer MergePlayer(BaseballPlayer existing, BaseballPlayer bhqData) =>
            existing == null
                ? bhqData
                : new BaseballPlayer 
                {
                    Id = existing.Id,
                    BhqId = existing.BhqId,
                    FirstName = existing.FirstName,
                    LastName = existing.LastName,
                    Age = MergePlayerValue(existing, bhqData, p => p.Age),
                    Type = existing.Type,
                    Positions = existing.Positions,
                    Team = MergePlayerValue(existing, bhqData, p => p.Team),
                    Status = existing.Status,
                    League1 = existing.League1,
                    League2 = existing.League2,
                    DraftRank = existing.DraftRank,
                    AverageDraftPick = existing.AverageDraftPick,
                    HighestPick = existing.HighestPick,
                    DraftedPercentage = existing.DraftedPercentage,
                    Reliability = bhqData.Reliability,
                    MayberryMethod = bhqData.MayberryMethod,
                    YearToDateBattingStats = new BattingStatsBuilder().AddStats(bhqData.YearToDateBattingStats).Build(),
                    YearToDatePitchingStats = new PitchingStatsBuilder().AddStats(bhqData.YearToDatePitchingStats).Build(),
                    ProjectedBattingStats = new BattingStatsBuilder().AddStats(bhqData.ProjectedBattingStats).Build(),
                    ProjectedPitchingStats = new PitchingStatsBuilder().AddStats(bhqData.ProjectedPitchingStats).Build(),
                    CombinedBattingStats = new BattingStatsBuilder()
                        .AddStats(bhqData.YearToDateBattingStats)
                        .AddStats(bhqData.ProjectedBattingStats)
                        .Build(),
                    CombinedPitchingStats = new PitchingStatsBuilder()
                        .AddStats(bhqData.YearToDatePitchingStats)
                        .AddStats(bhqData.ProjectedPitchingStats)
                        .Build()
                };
            
        private static List<BaseballPlayer> MergePlayers(Dictionary<BhqPlayerKey, BaseballPlayer> existingDictionary, 
                                                         IEnumerable<BaseballPlayer> batters,
                                                         IEnumerable<BaseballPlayer> pitchers)
        {
            var newData = new List<BaseballPlayer>();
            newData.AddRange(batters.Select(b => MergePlayer(FindAndRemovePlayer(existingDictionary, BuildKey(b)), b)));
            newData.AddRange(pitchers.Select(p => MergePlayer(FindAndRemovePlayer(existingDictionary, BuildKey(p)), p)));
            newData.AddRange(existingDictionary.Values.Select(e => MergePlayer(e, new BaseballPlayer())));
            return newData;
        }

        private static T MergePlayerValue<T>(BaseballPlayer existing, BaseballPlayer bhqData, Func<BaseballPlayer, T> getter) =>
            bhqData.BhqId > 0 ? getter(bhqData) : getter(existing);
    }
}