using System.Text.Json.Serialization;

namespace VyacheslavsMegaServer.Data.JSON.MCServerStat
{
    public class MCServerStat
    {
        public MCServerStat() 
        {
            Available = true;
        }

        public MCServerStat(bool statsAvailable)
        {
            Available = statsAvailable;
        }

        public bool Available { get; private set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("debug")]
        public DebugInfo Debug { get; set; }

        [JsonPropertyName("motd")]
        public MotdInfo Motd { get; set; }

        [JsonPropertyName("players")]
        public PlayersInfo Players { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("online")]
        public bool IsOnline { get; set; }

        [JsonPropertyName("protocol")]
        public int Protocol { get; set; }

        [JsonPropertyName("protocol_name")]
        public string ProtocolName { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }
}
