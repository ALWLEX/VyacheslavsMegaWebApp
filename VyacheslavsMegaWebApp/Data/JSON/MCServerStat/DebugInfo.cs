using System.Text.Json.Serialization;

namespace VyacheslavsMegaWebApp.Data.JSON.MCServerStat
{
    public class DebugInfo
    {
        [JsonPropertyName("ping")]
        public bool Ping { get; set; }

        [JsonPropertyName("query")]
        public bool Query { get; set; }

        [JsonPropertyName("srv")]
        public bool Srv { get; set; }

        [JsonPropertyName("querymismatch")]
        public bool QueryMismatch { get; set; }

        [JsonPropertyName("ipinsrv")]
        public bool IpInSrv { get; set; }

        [JsonPropertyName("cnameinsrv")]
        public bool CnameInSrv { get; set; }

        [JsonPropertyName("animatedmotd")]
        public bool AnimatedMotd { get; set; }

        [JsonPropertyName("cachehit")]
        public bool CacheHit { get; set; }

        [JsonPropertyName("cachetime")]
        public long CacheTime { get; set; }

        [JsonPropertyName("cacheexpire")]
        public long CacheExpire { get; set; }

        [JsonPropertyName("apiversion")]
        public int ApiVersion { get; set; }

        [JsonPropertyName("error")]
        public ErrorInfo Error { get; set; }
    }
}