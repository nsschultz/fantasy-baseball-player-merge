using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.CommonModels.Player;

namespace FantasyBaseball.PlayerMergeService.Services
{
    /// <summary>Service for updating the players.</summary>
    public interface IPlayerUpdaterService
    {
        /// <summary>Updates the players.</summary>
        /// <param name="players">The updated players data.</param>
        Task UpdatePlayers(IEnumerable<BaseballPlayer> players);
    }
}