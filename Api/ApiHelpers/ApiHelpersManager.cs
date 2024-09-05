using KinescopeTesting.Api.ApiHelpers.CommonApiHelpers;
using KinescopeTesting.Api.ApiHelpers.KinescopeApi;

namespace KinescopeTesting.Api.ApiHelpers
{
    public static class ApiHelpersManager
    {
        public static VideoDownloadApiHelper VideoDownloadApiHelper = new();
        public static ProjectsApiHelper ProjectsApiHelper = new();
        public static HtmlApiHelper HtmlApiHelper = new();
    }
}

