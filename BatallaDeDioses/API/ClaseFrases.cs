using System.Text.Json.Serialization;
namespace ConsumoAPI
{
    public class FraseAPI
    {
        [JsonPropertyName("number")]
        public string number { get; set; }

        [JsonPropertyName("language")]
        public string language { get; set; }

        [JsonPropertyName("insult")]
        public string insult { get; set; }

        [JsonPropertyName("created")]
        public string created { get; set; }

        [JsonPropertyName("shown")]
        public string shown { get; set; }

        [JsonPropertyName("createdby")]
        public string createdby { get; set; }

        [JsonPropertyName("active")]
        public string active { get; set; }

        [JsonPropertyName("comment")]
        public string comment { get; set; }
    }
}

