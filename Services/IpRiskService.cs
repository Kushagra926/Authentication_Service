using Authentication_Servie.Helpers;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Caching.Distributed;

namespace Authentication_Servie.Services
{
    public class IpRiskService
    {

        private readonly IDistributedCache _cache;
        private readonly QuestDbService _questDbService;
        public IpRiskService(IDistributedCache cache, QuestDbService questDbService)
        {
            _cache = cache;
            _questDbService = questDbService;
        }
        public async Task<int> GetIpRiskAsync(string ip)
        {
            var value  = await _cache.GetStringAsync($"ip_risk:{ip}");
            return value == null ? 0: int.Parse(value);

        }

        public async Task IncreaseIpRiskAsync(string ip, int amount)
        {
            var risk = await GetIpRiskAsync(ip);

            risk += amount;

            await _cache.SetStringAsync(
                $"ip_risk:{ip}",
                risk.ToString(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });

            if (risk >= 50)
                await BlockIpAsync(ip);
        }

        public async Task BlockIpAsync(string ip)
        {
            if (IpBlockBypassHelper.IsInternalServiceIp(ip))
                return;
            await _cache.SetStringAsync(
                $"blocked_ip:{ip}",
                "1",
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                });
            await _questDbService.LogAuthEventAsync(
                           0,
                           "N/A",
                           "SYSTEM",
                           "IP_BLOCKED",
                           ip,
                           "auto"
);

        }
    }
}
