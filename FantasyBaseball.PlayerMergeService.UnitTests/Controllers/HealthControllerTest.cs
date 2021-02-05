using FantasyBaseball.Common.Exceptions;
using FantasyBaseball.PlayerMergeService.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace FantasyBaseball.PlayerMergeService.Controllers.UnitTests
{
    public class HealthControllerTest
    {
        [Fact] public void BadBhqStatsService() 
        {
            var bhqStatsSection = new Mock<IConfigurationSection>();
            bhqStatsSection.Setup(o => o.Value).Returns("bhq-stats-url");
            var playerSection = new Mock<IConfigurationSection>();
            playerSection.Setup(o => o.Value).Returns("player-url");
            var config = new Mock<IConfiguration>();
            config.Setup(o => o.GetSection("ServiceUrls:BhqStatsService")).Returns(bhqStatsSection.Object);
            config.Setup(o => o.GetSection("ServiceUrls:PlayerService")).Returns(playerSection.Object);
            var service = new Mock<IHealthCheckService>();
            service.Setup(o => o.CheckHealth("bhq-stats-url")).Throws(new ServiceException("Bhq Service Unhealthy"));
            service.Setup(o => o.CheckHealth("player-url"));
            var e = Assert.Throws<ServiceException>(() => new HealthController(config.Object, service.Object).GetHealth());
            Assert.Equal("Bhq Service Unhealthy", e.Message);
        }

        [Fact] public void BadPlayerService() 
        {
            var bhqStatsSection = new Mock<IConfigurationSection>();
            bhqStatsSection.Setup(o => o.Value).Returns("bhq-stats-url");
            var playerSection = new Mock<IConfigurationSection>();
            playerSection.Setup(o => o.Value).Returns("player-url");
            var config = new Mock<IConfiguration>();
            config.Setup(o => o.GetSection("ServiceUrls:BhqStatsService")).Returns(bhqStatsSection.Object);
            config.Setup(o => o.GetSection("ServiceUrls:PlayerService")).Returns(playerSection.Object);
            var service = new Mock<IHealthCheckService>();
            service.Setup(o => o.CheckHealth("bhq-stats-url"));
            service.Setup(o => o.CheckHealth("player-url")).Throws(new ServiceException("Player Service Unhealthy"));
            var e = Assert.Throws<ServiceException>(() => new HealthController(config.Object, service.Object).GetHealth());
            Assert.Equal("Player Service Unhealthy", e.Message);
        }

        [Fact] public void ValidServices() 
        {
            var bhqStatsSection = new Mock<IConfigurationSection>();
            bhqStatsSection.Setup(o => o.Value).Returns("bhq-stats-url");
            var playerSection = new Mock<IConfigurationSection>();
            playerSection.Setup(o => o.Value).Returns("player-url");
            var config = new Mock<IConfiguration>();
            config.Setup(o => o.GetSection("ServiceUrls:BhqStatsService")).Returns(bhqStatsSection.Object);
            config.Setup(o => o.GetSection("ServiceUrls:PlayerService")).Returns(playerSection.Object);
            var service = new Mock<IHealthCheckService>();
            service.Setup(o => o.CheckHealth("bhq-stats-url"));
            service.Setup(o => o.CheckHealth("player-url"));
            new HealthController(config.Object, service.Object).GetHealth();
            service.VerifyAll();
        }
    }
}