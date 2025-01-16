using System.Net.Http;
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
                //User-Agent is required by API
                apiHttpClient.DefaultRequestHeaders.Add("User-Agent", "Vyacheslav's mega server web app");
                apiHttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                
                HttpResponseMessage response;
                try
                {
                    response = await apiHttpClient.GetAsync(MCServerStatApiUrl);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    return null;
                }

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
