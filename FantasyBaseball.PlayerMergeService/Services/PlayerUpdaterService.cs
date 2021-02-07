using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyBaseball.Common.Exceptions;
using FantasyBaseball.Common.Models;
using Microsoft.Extensions.Configuration;

namespace FantasyBaseball.PlayerMergeService.Services
{
    /// <summary>Service for updating the players.</summary>
    public class PlayerUpdaterService : IPlayerUpdaterService
    {
        private readonly string _url;

        /// <summary>Creates a new instance of the service.</summary>
        /// <param name="configuration">The configuration for the application.</param>
        public PlayerUpdaterService(IConfiguration configuration) => _url = configuration.GetValue<string>("ServiceUrls:PlayerService");

        /// <summary>Updates the players.</summary>
        /// <param name="players">The updated players data.</param>
        public async Task UpdatePlayers(IEnumerable<BaseballPlayer> players)
        {
            var json = JsonSerializer.Serialize(new PlayerCollection { Players = players.ToList() });
            var response = await new HttpClient().PostAsync($"{_url}/player", new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) throw new ServiceException("Unable to upsert players");
        }
    }
}