using System.Text.Json;
using System.Net.Http;
using System.Text;

namespace Authentication_Servie.Services
{
    public class QuestDbService
    {
        private readonly HttpClient _httpClient;

        public QuestDbService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:9000")
            };
        }

        public async Task LogAuthEventAsync(
            int userId,
            string email,
            string provider,
            string event_type,
            string ip,
            string user_agent
            )
        {
            var sql = $@"
INSERT INTO auth_events
(ts, user_id, email, provider, event_type, ip, user_agent)
VALUES(
now(),
'{userId}',
'{email}',
'{provider}',
'{event_type}',
'{ip}',
'{user_agent}'
)";
            //    var payload = new StringContent(sql, Encoding.UTF8, "text/plain");
            //    var response = await _httpClient.PostAsync("/exec", payload);
            //    var body = await response.Content.ReadAsStringAsync();

            //    if (!response.IsSuccessStatusCode)
            //        throw new Exception(body);
            //    //await _httpClient.PostAsync("/exec", payload);
            //}

            var url = $"/exec?query={Uri.EscapeDataString(sql)}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"QuestDB insert failed: {body}");
            }
        }



        public async Task<List<string>> GetBruteForceIpsAsync()
        {
            var query = @"
        SELECT ip
        FROM auth_events
        WHERE event_type = 'LOGIN_FAILED'
        AND ts > now() - 10m
        GROUP BY ip
        HAVING count(*) >= 5
    ";

            var response = await _httpClient.GetStringAsync($"/exec?query={Uri.EscapeDataString(query)}");

            // parse response → extract IPs
            return ParseIps(response);
        }


        public async Task LogSecurityEventAsync(string ip, string type)
        {
            var sql = $@"
        INSERT INTO auth_events
(ts, user_id, email, provider, event_type, ip, user_agent)
        VALUES (
            now(),
            -1,
            'system',
            'SYSTEM',
            '{type}',
            '{ip}',
            'SecurityMonitor'
        )
    ";
            var url = $"/exec?query={Uri.EscapeDataString(sql)}";
            await _httpClient.GetAsync(url);

            //await _httpClient.PostAsync("/exec", new StringContent(sql));
        }

        private List<string> ParseIps(string json)
        {
            var ips = new List<string>();

            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("dataset", out var dataset))
                return ips;

            foreach (var row in dataset.EnumerateArray())
            {
                if (row.GetArrayLength() > 0)
                {
                    var ip = row[0].GetString();
                    if (!string.IsNullOrWhiteSpace(ip))
                        ips.Add(ip);
                }
            }

            return ips;
        }


    }
}
