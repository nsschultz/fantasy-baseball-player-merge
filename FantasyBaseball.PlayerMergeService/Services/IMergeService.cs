using System.Collections.Generic;
using FantasyBaseball.CommonModels.Player;

namespace FantasyBaseball.PlayerMergeService.Services
{
    /// <summary>The service for merging the new stats to the existing players.</summary>
    public interface IMergeService
    {
        /// <summary>Merges the new stats into the existing player list.</summary>
        /// <param name="existingPlayers">The existing collection of players.</param>
        /// <param name="batters">The new batting stats data</param>
        /// <param name="pitchers">The new pitching stats data.</param>
        /// <returns>The merged collection of players.</returns>
        List<BaseballPlayer> MergePlayers(IEnumerable<BaseballPlayer> existingPlayers, 
                                          IEnumerable<BaseballPlayer> batters, 
                                          IEnumerable<BaseballPlayer> pitchers);
    }
}