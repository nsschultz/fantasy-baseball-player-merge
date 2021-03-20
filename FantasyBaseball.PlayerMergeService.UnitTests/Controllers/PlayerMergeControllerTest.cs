using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PlayerMergeService.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace FantasyBaseball.PlayerMergeService.Controllers.UnitTests
{
    public class PlayerMergeControllerTest
    {
        [Fact] public async void MergePlayersTest() 
        {
            var playerServiceSection = new Mock<IConfigurationSection>();
            playerServiceSection.Setup(o => o.Value).Returns("player-service");
            var bhqStatsSection = new Mock<IConfigurationSection>();
            bhqStatsSection.Setup(o => o.Value).Returns("bhq-stats-service");
            var config = new Mock<IConfiguration>();
            config.Setup(o => o.GetSection("ServiceUrls:PlayerService")).Returns(playerServiceSection.Object);
            config.Setup(o => o.GetSection("ServiceUrls:BhqStatsService")).Returns(bhqStatsSection.Object);
            var merge = new Mock<IMergeService>();
            merge.Setup(o => o.MergePlayers(
                It.Is<List<BaseballPlayer>>(p => p.First().BhqId == 1),
                It.Is<List<BaseballPlayer>>(p => p.First().BhqId == 2),
                It.Is<List<BaseballPlayer>>(p => p.First().BhqId == 3)
            )).Returns(new List<BaseballPlayer> { BuildTestPlayer(1), BuildTestPlayer(2), BuildTestPlayer(3) });
            var getter = new Mock<IDataGetterService>();
            getter.Setup(o => o.GetData<PlayerCollection>("player-service/player")).ReturnsAsync(new PlayerCollection { Players = BuildTestPlayerList(1) });
            getter.Setup(o => o.GetData<IEnumerable<BaseballPlayer>>("bhq-stats-service/bhq-stats/batters")).ReturnsAsync(BuildTestPlayerList(2));
            getter.Setup(o => o.GetData<IEnumerable<BaseballPlayer>>("bhq-stats-service/bhq-stats/pitchers")).ReturnsAsync(BuildTestPlayerList(3));
            var update = new Mock<IPlayerUpdaterService>();
            update.Setup(o => o.UpdatePlayers(It.Is<List<BaseballPlayer>>(p => p.Count == 3))).Returns(Task.FromResult(0));
            await new PlayerMergeController(config.Object, merge.Object, getter.Object, update.Object).MergePlayers();
            getter.VerifyAll();
            update.VerifyAll();
        }

        private static BaseballPlayer BuildTestPlayer(int id) => new BaseballPlayer { BhqId = id };

        private static List<BaseballPlayer> BuildTestPlayerList(int id) => new List<BaseballPlayer> { new BaseballPlayer { BhqId = id } };
    }
}