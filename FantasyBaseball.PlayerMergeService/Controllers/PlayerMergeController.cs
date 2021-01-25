using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyBaseball.CommonModels.Player;
using FantasyBaseball.PlayerMergeService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FantasyBaseball.PlayerMergeService.Controllers
{
    /// <summary>Endpoint for gathering the player stats data and merging it into the existing data.</summary>
    [Route("api/player/merge")] [ApiController] public class PlayerMergeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDataGetterService _getterService;
        private readonly IMergeService _mergeService;
        private readonly IPlayerUpdaterService _updaterService;

        /// <summary>Creates a new instance of the controller.</summary>
        /// <param name="configuration">The configuration for the application.</param>
        /// <param name="mergeService">The service for merging the new stats to the existing players.</param>
        /// <param name="getterService">Service for getting the data from other services.</param>
        /// <param name="updaterService">Service for updating the players.</param>
        public PlayerMergeController(IConfiguration configuration, 
                                     IMergeService mergeService, 
                                     IDataGetterService getterService, 
                                     IPlayerUpdaterService updaterService) 
        { 
            _configuration = configuration;
            _mergeService = mergeService;
            _getterService = getterService;
            _updaterService = updaterService;
        }
        
        /// <summary>Gathers the data and merges it.</summary>
        [HttpPost] public async Task MergePlayers() 
        {
            var existingPlayers = await _getterService.GetData<PlayerCollection>($"{_configuration.GetValue<string>("ServiceUrls:PlayerService")}/player");
            var battingStats = await _getterService.GetData<IEnumerable<BaseballPlayer>>($"{_configuration.GetValue<string>("ServiceUrls:BhqStatsService")}/bhq-stats/batters");
            var pitchingStats = await _getterService.GetData<IEnumerable<BaseballPlayer>>($"{_configuration.GetValue<string>("ServiceUrls:BhqStatsService")}/bhq-stats/pitchers");
            var mergedData = _mergeService.MergePlayers(existingPlayers?.Players, battingStats, pitchingStats);
            await _updaterService.UpdatePlayers(mergedData);
        }
    }
}