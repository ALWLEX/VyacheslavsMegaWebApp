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

                    // Deserialize JSON to object
                    return JsonSerializer.Deserialize<MCServerStat.MCServerStat>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Ignore case of properties' names
                    }) ?? new MCServerStat.MCServerStat(false);
                }
                catch (Exception ex)
                {
                    return new MCServerStat.MCServerStat(false);
                }
            }
        }
    }
}
