using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authentication_Servie.Services;

namespace Authentication_Servie.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/security")]
    //[Authorize(Roles = "Admin")]
    public class SecurityMetricsController : ControllerBase
    {
        private readonly SecurityMetricsService _metrics;
        public SecurityMetricsController(SecurityMetricsService metrics)
        {
            _metrics = metrics;
        }
        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            var logins = await _metrics.GetLoginsLast5MinAsync();
            var failed = await _metrics.GetFailedLoginsLast5MinAsync();
            var blockedIps = await _metrics.GetBlockedIpsLastHourAsync();
            var providers = await _metrics.GetAuthProviderStatsAsync();
            var attackers = await _metrics.GetTopAttackersAsync();

            return Ok(new
            {
                loginsLast5min = logins,
                failedLoginsLast5min = failed,
                blockedIpsLast5min = blockedIps,
                authProviders = providers,
                topAttackers = attackers
            });
        }
    }
}
