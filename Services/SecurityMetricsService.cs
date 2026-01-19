using Authentication_Servie.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Authentication_Servie.Services
{
    public class SecurityMetricsService
    {
        private readonly HttpClient _http;

        public SecurityMetricsService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:9000"); 
        }

        //private async Task<string> QueryAsync(string sql)
        //{
        //    var payload = new StringContent(sql, Encoding.UTF8, "text/plain");
        //    var response = await _http.PostAsync("/exec", payload);
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}

        private async Task<string> QueryAsync(string sql)
        {
            var url = $"/exec?query={Uri.EscapeDataString(sql)}";

            var response = await _http.GetAsync(url);   
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<long> GetLoginsLast5MinAsync()
        {
            var json = await QueryAsync(@"
        SELECT count(*)
        FROM auth_events
        WHERE event_type = 'LOGIN_SUCCESS'
        AND ts > dateadd('m', -5, now());
    ");

            return ParseSingleValue(json);

        }

        public async Task<long> GetFailedLoginsLast5MinAsync()
        {
            var json = await QueryAsync(@"
        SELECT count(*)
        FROM auth_events
        WHERE event_type = 'LOGIN_FAILED'
        AND ts > dateadd('m', -5, now());
    ");

            return ParseSingleValue(json);

        }

        public async Task<long> GetBlockedIpsLastHourAsync()
        {
            var json = await QueryAsync(@"
        SELECT count(DISTINCT ip)
        FROM auth_events
        WHERE event_type = 'IP_BLOCKED'
        AND ts > dateadd('h', -1, now());
    ");

            return ParseSingleValue(json);


        }

        public async Task<List<AttackerDto>> GetTopAttackersAsync()
        {
            var json = await QueryAsync(@"
        SELECT ip, count(*) AS attempts
        FROM auth_events
        WHERE event_type = 'LOGIN_FAILED'
        AND ts > dateadd('h', -1, now())
        GROUP BY ip
        ORDER BY attempts DESC
        LIMIT 5;
    ");

            return ParseList<AttackerDto>(json);
        }

        public async Task<List<AuthProviderStatDto>> GetAuthProviderStatsAsync()
        {
            var json = await QueryAsync(@"
        SELECT provider, count(*) AS count
        FROM auth_events
        WHERE ts > dateadd('h', -24, now())
        GROUP BY provider;
    ");

            return ParseList<AuthProviderStatDto>(json);
        }



        //private T ParseSingleValue<T>(string json)
        //{
        //    var result = JsonSerializer.Deserialize<QuestDbResponse<T>>(json);
        //    return result!.dataset[0][0];
        //}

        private long ParseSingleValue(string json)
        {
            using var doc = JsonDocument.Parse(json);

            var dataset = doc.RootElement.GetProperty("dataset");

            if (dataset.GetArrayLength() == 0)
                return 0;

            var value = dataset[0][0];

            return value.ValueKind switch
            {
                JsonValueKind.Number => value.GetInt64(),
                _ => 0
            };
        }


        private List<T> ParseList<T>(string json) where T : new()
        {
            using var doc = JsonDocument.Parse(json);
            var dataset = doc.RootElement.GetProperty("dataset");
            var columns = doc.RootElement.GetProperty("columns");

            var list = new List<T>();

            foreach (var row in dataset.EnumerateArray())
            {
                var obj = new T();
                var props = typeof(T).GetProperties();

                for (int i = 0; i < row.GetArrayLength(); i++)
                {
                    var columnName = columns[i].GetProperty("name").GetString();
                    var prop = props.FirstOrDefault(p =>
                        p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

                    if (prop == null) continue;

                    var cell = row[i];

                    object? value = cell.ValueKind switch
                    {
                        JsonValueKind.String => cell.GetString(),
                        JsonValueKind.Number => cell.GetInt64(),
                        _ => null
                    };

                    prop.SetValue(obj, value);
                }

                list.Add(obj);
            }

            return list;
        }



        //private List<T> ParseList<T>(string json)
        //{
        //    var doc = JsonDocument.Parse(json);
        //    var dataset = doc.RootElement.GetProperty("dataset");

        //    var list = new List<T>();

        //    foreach (var row in dataset.EnumerateArray())
        //    {
        //        list.Add(JsonSerializer.Deserialize<T>(row.GetRawText())!);
        //    }

        //    return list;
        //}


        // Logins last 5 minutes
        //public Task<string> GetLoginsLast5MinAsync()
        //{
        //    return QueryAsync(@"
        //        SELECT count(*) 
        //        FROM auth_events 
        //        WHERE event_type = 'LOGIN_SUCCESS'
        //        AND ts > dateadd('m', -5, now());
        //    ");
        //}

        //// Failed logins last 5 minutes
        //public Task<string> GetFailedLoginsLast5MinAsync()
        //{
        //    return QueryAsync(@"
        //        SELECT count(*) 
        //        FROM auth_events 
        //        WHERE event_type = 'LOGIN_FAILED'
        //        AND ts > dateadd('m', -5, now());
        //    ");
        //}

        //// Blocked IPs last 1 hour
        //public Task<string> GetBlockedIpsLastHourAsync()
        //{
        //    return QueryAsync(@"
        //        SELECT count(DISTINCT ip) 
        //        FROM auth_events 
        //        WHERE event_type = 'IP_BLOCKED'
        //        AND ts > dateadd('h', -1, now());
        //    ");
        //}

        //// OAuth vs Local
        //public Task<string> GetAuthProviderStatsAsync()
        //{
        //    return QueryAsync(@"
        //        SELECT provider, count(*) 
        //        FROM auth_events 
        //        WHERE ts > dateadd('h', -24, now())
        //        GROUP BY provider;
        //    ");
        //}

        //// Top attacking IPs
        //public Task<string> GetTopAttackersAsync()
        //{
        //    return QueryAsync(@"
        //        SELECT ip, count(*) AS attempts
        //        FROM auth_events
        //        WHERE event_type = 'LOGIN_FAILED'
        //        AND ts > dateadd('h', -1, now())
        //        GROUP BY ip
        //        ORDER BY attempts DESC
        //        LIMIT 5;
        //    ");
        //}
    }
}
