using System.Text.Json;
using VyacheslavsMegaServer.Service;

namespace VyacheslavsMegaServer.Data.JSON.Repositories
{
    public class MCServerStatRepository
    {
        public async Task<MCServerStat.MCServerStat> GetMCServerStats()
        {
            string MCServerStatApiUrl = $"https://api.mcsrvstat.us/2/{MCServerData.IP}:{MCServerData.Port}";
            MCServerStat.MCServerStat? MCServerStat;
            using (HttpClient apiHttpClient = new HttpClient())
            {
                HttpResponseMessage response = await apiHttpClient.GetAsync(MCServerStatApiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Десериализуем JSON в объект C#
                MCServerStat = JsonSerializer.Deserialize<MCServerStat.MCServerStat>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Игнорируем регистр имён свойств
                });
            }

            return MCServerStat;
        }
    }
}
