using System.Text.Json.Serialization;

namespace VyacheslavsMegaServer.Data.JSON.MCServerStat
{
    public class MotdInfo
    {
        [JsonPropertyName("raw")]
        public List<string> Raw { get; set; }

        [JsonPropertyName("clean")]
        public List<string> Clean { get; set; }

        [JsonPropertyName("html")]
        public List<string> Html { get; set; }
    }
}