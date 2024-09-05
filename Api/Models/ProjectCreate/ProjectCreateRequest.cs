using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace KinescopeTesting.Api.ApiTypes.ProjectCreate
{
    public class ProjectCreateRequest : BaseModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("privacy_type")]
        public string PrivacyType { get; set; }

        [JsonPropertyName("privacy_domains")]
        public List<string> PrivacyDomains { get; set; }

        [JsonPropertyName("player_id")]
        public Guid? PlayerId { get; set; }

        [JsonPropertyName("encrypted")]
        public bool? Encrypted { get; set; }

        [JsonPropertyName("catalog_type")]
        public string CatalogType { get; set; }

        public static ProjectCreateRequest FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException(nameof(json), "JSON string cannot be null or empty");
            }

            return JsonConvert.DeserializeObject<ProjectCreateRequest>(json);
        }
    }
}