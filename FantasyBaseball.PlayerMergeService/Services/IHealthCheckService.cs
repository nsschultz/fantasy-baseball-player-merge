using System.Threading.Tasks;

namespace FantasyBaseball.PlayerMergeService.Services
{
    /// <summary>Service for checking the health of another service.</summary>
    public interface IHealthCheckService
    {
        /// <summary>Checks the health of aservice</summary>
        /// <param name="url">The url to check.</param>
        Task CheckHealth(string url);
    }
}