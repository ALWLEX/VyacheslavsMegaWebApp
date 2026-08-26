using System.Text.Json.Serialization;

namespace VyacheslavsMegaWebApp.Data.JSON.MCServerStat
{
    public class ErrorInfo
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }
    }
}