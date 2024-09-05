using KinescopeTesting.Api.ApiTypes;
using KinescopeTesting.Api.ApiTypes.ProjectCreate;
using RestSharp;

namespace KinescopeTesting.Api.ApiHelpers.KinescopeApi
{
    public class ProjectsApiHelper : BaseKinescopeApiHelper
    {
        private const string Url = "https://api.kinescope.io/v1/projects/";
        public ProjectsApiHelper() : base()
        { }

        public async Task<ProjectCreateResponse> CreateProjectAsync(ProjectCreateRequest apiType)
        {
            var request = new RestRequest($"{Url}");

            request.AddJsonBody(apiType);

            var response = await SendRequestAsync(request, Method.Post);

            var responseApiType = BaseModel.FromJson<ProjectCreateResponse>(response.Content);

            return responseApiType;
        }

        public async Task<RestResponse> DeleteProjectAsync(string projectId)
        {
            var request = new RestRequest($"{Url}{projectId}");

            var response = await SendRequestAsync(request, Method.Delete);

            return response;
        }
    }
}
