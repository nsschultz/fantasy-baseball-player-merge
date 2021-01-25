using System.Net.Http;
using System.Threading.Tasks;
using FantasyBaseball.CommonModels.Exceptions;

namespace FantasyBaseball.PlayerMergeService.Services
{
    /// <summary>Service for checking the health of another service.</summary>
    public class HealthCheckService : IHealthCheckService
    {
        /// <summary>Checks the health of aservice</summary>
        /// <param name="url">The url to check.</param>
        public async Task CheckHealth(string url)
        {
            var response = await new HttpClient().GetAsync($"{url}/health");
            if (!response.IsSuccessStatusCode) throw new ServiceException($"Unable to get {url}");
        }
    }
}