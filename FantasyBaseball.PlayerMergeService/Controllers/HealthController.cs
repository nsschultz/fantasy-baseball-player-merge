using FantasyBaseball.PlayerMergeService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FantasyBaseball.PlayerMergeService.Controllers
{
    /// <summary>Endpoint for checking the health of the service.</summary>
    [Route("api/health")] [ApiController] public class HealthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHealthCheckService _service;

        /// <summary>Creates a new instance of the controller.</summary>
        /// <param name="configuration">The configuration for the application.</param>
        /// <param name="service">Service for checking the health of another service.</param>
        public HealthController(IConfiguration configuration, IHealthCheckService service) 
        { 
            _configuration = configuration;
            _service = service;
        }
        
        /// <summary>Ensures that the system has access to the services.</summary>
        [HttpGet] public void GetHealth() 
        {
            _service.CheckHealth(_configuration.GetValue<string>("ServiceUrls:BhqStatsService"));
            _service.CheckHealth(_configuration.GetValue<string>("ServiceUrls:PlayerService"));
        }
    }
}