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
VALUES(
now(),
'{email}',
'{provider}',
'{event_type}',
'{ip}',
'{user_agent}'
)";
            var payload = new StringContent(sql, Encoding.UTF8, "text/plain");

            await _httpClient.PostAsync("/exec", payload);
        }

    }
}
