using Newtonsoft.Json;

namespace KinescopeTesting.Api.ApiTypes.UploadFile
{
    public class UploadFileRequest : BaseModel
    {
        [JsonProperty("data")]
        public Data ApiTypeData { get; set; }

        public class Data
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("parent_id")]
            public string ParentId { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("play_link")]
            public string PlayLink { get; set; }

            [JsonProperty("embed_link")]
            public string EmbedLink { get; set; }

            [JsonProperty("hls_link")]
            public string HlsLink { get; set; }

            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }
        }
    }
}