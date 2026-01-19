using Authentication_Servie.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

public class SecurityMonitoringService : BackgroundService
{
    private readonly QuestDbService _questDb;
    private readonly IDistributedCache _cache;
    private readonly ILogger<SecurityMonitoringService> _logger;

    public SecurityMonitoringService(
        QuestDbService questDb,
        IDistributedCache cache,
        ILogger<SecurityMonitoringService> logger)
    {
        _questDb = questDb;
        _cache = cache;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Security Monitoring Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DetectBruteForceAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Security monitoring failed");
            }

            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        }
    }

    private async Task DetectBruteForceAsync()
    {
        var suspiciousIps = await _questDb.GetBruteForceIpsAsync();

        foreach (var ip in suspiciousIps)
        {
            var cacheKey = $"blocked_ip:{ip}";

            if (await _cache.GetStringAsync(cacheKey) != null)
                continue; // already blocked

            await _cache.SetStringAsync(
                cacheKey,
                "BLOCKED",
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                });

            _logger.LogWarning($"Blocked IP due to brute force: {ip}");

            await _questDb.LogSecurityEventAsync(
                ip,
                "BRUTE_FORCE_BLOCK"
            );
        }
    }



}
