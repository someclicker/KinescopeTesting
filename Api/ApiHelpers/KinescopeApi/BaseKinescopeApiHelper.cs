using RestSharp;

namespace KinescopeTesting.Api.ApiHelpers.KinescopeApi
{
    public class BaseKinescopeApiHelper : BaseApi
    {
        private readonly string ApiToken;
        private const string ApiTokenEnvironmentVariable = "API_TOKEN";

        protected static class ApiHeaders
        {
            public const string VideoUrl = "X-Video-URL";
            public const string ParentId = "X-Parent-ID";
            public const string VideoTitle = "X-Video-Title";
            public const string Authorization = "Authorization";
        }

        public BaseKinescopeApiHelper()
        {
            RestClient = new RestClient();

            ApiToken = "656bfa74-9313-4409-be23-bca9e86d485c";

            ApiToken = Environment.GetEnvironmentVariable(ApiTokenEnvironmentVariable)
                ?? throw new InvalidOperationException("API token is missing." +
                " Please ensure that the 'API_TOKEN' environment variable is set.");

            RestClient.AddDefaultHeader(ApiHeaders.Authorization, $"{AuthTokenNames.Bearer} {ApiToken}");
        }
    }
}
