using Microsoft.Extensions.Caching.Distributed;

namespace Authentication_Servie.Services
{
    public class RiskScoringService
    {
        private readonly IDistributedCache _cache;

        public RiskScoringService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<int> GetUserRiskAsync(int userId)
        {
            var value = await _cache.GetStringAsync($"risk:user:{userId}");
            return value == null ? 0 : int.Parse(value);
        }

        public async Task IncreaseUserRiskAsync(int userId, int points)
        {
            var current = await GetUserRiskAsync(userId);
            var updated = Math.Min(current + points, 100);

            await _cache.SetStringAsync(
                $"risk:user:{userId}",
                updated.ToString(),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(24)
                });
        }

        public async Task ReduceUserRiskAsync(int userId, int points)
        {
            var current = await GetUserRiskAsync(userId);
            var updated = Math.Max(current - points, 0);

            await _cache.SetStringAsync(
                $"risk:user:{userId}",
                updated.ToString(),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(24)
                });
        }
    }

}
