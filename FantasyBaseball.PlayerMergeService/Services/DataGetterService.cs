using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyBaseball.Common.Exceptions;

namespace FantasyBaseball.PlayerMergeService.Services
{
    /// <summary>Service for getting the data from other services.</summary>
    public class DataGetterService : IDataGetterService
    {
        /// <summary>Gets all of the data from the source.</summary>
        /// <param name="url">The url to retrieve the data from.</param>
        /// <returns>All of the data from the source.</returns>
        public async Task<T> GetData<T>(string url)
        {
            var response = await new HttpClient().GetAsync(url);
            if (!response.IsSuccessStatusCode) throw new ServiceException($"Unable to get {url}");
            return await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(), 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}