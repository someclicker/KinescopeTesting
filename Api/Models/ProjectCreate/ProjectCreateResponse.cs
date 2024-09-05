using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace KinescopeTesting.Api.ApiTypes
{
    public class ProjectCreateResponse : BaseModel
    {
        [JsonPropertyName("data")]
        public ProjectData Data { get; set; }

        public class ProjectData
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("folders")]
            public List<object> Folders { get; set; }

            [JsonPropertyName("privacy_type")]
            public string PrivacyType { get; set; }

            [JsonPropertyName("privacy_domains")]
            public List<string> PrivacyDomains { get; set; }

            [JsonPropertyName("player_id")]
            public string PlayerId { get; set; }

            [JsonPropertyName("favorite")]
            public bool Favorite { get; set; }

            [JsonPropertyName("size")]
            public int Size { get; set; }

            [JsonPropertyName("items_count")]
            public int ItemsCount { get; set; }

            [JsonPropertyName("created_at")]
            public DateTime CreatedAt { get; set; }

            [JsonPropertyName("updated_at")]
            public DateTime? UpdatedAt { get; set; }

            [JsonPropertyName("encrypted")]
            public bool Encrypted { get; set; }
        }
    }
}