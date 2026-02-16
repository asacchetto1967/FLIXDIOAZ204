using System;
using System.Text.Json.Serialization;

namespace FlixDioAZ204.Models
{
    public class Movie
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }

        [JsonPropertyName("videoUrl")]
        public string VideoUrl { get; set; }

        [JsonPropertyName("thumbUrl")]
        public string ThumbUrl { get; set; }
    }
}
