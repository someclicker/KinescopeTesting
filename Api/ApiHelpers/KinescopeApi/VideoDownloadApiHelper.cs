using KinescopeTesting.Api.ApiTypes;
using KinescopeTesting.Api.ApiTypes.UploadFile;
using RestSharp;

namespace KinescopeTesting.Api.ApiHelpers.KinescopeApi
{
    public class VideoDownloadApiHelper : BaseKinescopeApiHelper
    {
        private const string UploadVideoUrl = "https://uploader.kinescope.io/v2/video";
        public VideoDownloadApiHelper() : base()
        { }

        public async Task<UploadFileRequest> UploadVideoByLinkAsync(string fileLink, string title, string parentId)
        {
            var request = new RestRequest(UploadVideoUrl);

            request.AddHeader(ApiHeaders.VideoUrl, fileLink);
            request.AddHeader(ApiHeaders.ParentId, parentId);
            request.AddHeader(ApiHeaders.VideoTitle, title);

            var response = await SendRequestAsync(request, Method.Post);

            return BaseModel.FromJson<UploadFileRequest>(response.Content);
        }
    }
}
