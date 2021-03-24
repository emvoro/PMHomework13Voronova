using System.Text.Json.Serialization;

namespace DepsTest.Models
{
    public class CustomUri
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
