using Newtonsoft.Json;

namespace KinescopeTesting.Api.ApiTypes
{
    public abstract class BaseModel
    {
        public static T FromJson<T>(string json) where T : BaseModel
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException(nameof(json), "JSON string cannot be null or empty");
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
