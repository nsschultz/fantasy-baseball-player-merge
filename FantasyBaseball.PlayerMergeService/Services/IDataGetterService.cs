using System.Threading.Tasks;

namespace FantasyBaseball.PlayerMergeService.Services
{
    /// <summary>Service for getting the data from other services.</summary>
    public interface IDataGetterService
    {
        /// <summary>Gets all of the data from the source.</summary>
        /// <param name="url">The url to retrieve the data from.</param>
        /// <returns>All of the data from the source.</returns>
        Task<T> GetData<T>(string url);
    }
}