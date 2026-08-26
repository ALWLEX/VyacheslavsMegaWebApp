using System.Text.Json.Serialization;

namespace VyacheslavsMegaWebApp.Data.JSON.MCServerStat
{
    public class PlayersInfo
    {
        [JsonPropertyName("online")]
        public int Online { get; set; }

        [JsonPropertyName("max")]
        public int Max { get; set; }
    }
}