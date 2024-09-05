using RestSharp;
using Serilog;

namespace KinescopeTesting.Api.ApiHelpers
{       
    public class BaseApi
    {
        protected static class AuthTokenNames
        {
            public const string Bearer = "Bearer";
        }

        protected RestClient RestClient;

        public BaseApi()
        {
            RestClient = new RestClient();
        }

        public async Task<RestResponse> SendRequestAsync(RestRequest request, Method requestMethod)
        {
            request.Method = requestMethod;

            Log.Information("Отправка запроса: {Method} {Resource}", request.Method, request.Resource);

            RestResponse response;

            try
            {
                response = await RestClient.ExecuteAsync(request);

                Log.Information("Ответ получен: {StatusCode} для {Resource}", response.StatusCode, request.Resource);

                if (!response.IsSuccessful)
                {
                    Log.Error("Запрос завершился ошибкой: {StatusCode} {ErrorMessage}", response.StatusCode, response.ErrorMessage);
                    throw new Exception($"Request failed with status code {response.StatusCode}: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при отправке запроса: {Method} {Resource}", request.Method, request.Resource);
                throw;
            }

            return response;
        }
    }
}