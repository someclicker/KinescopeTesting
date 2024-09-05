using HtmlAgilityPack;
using KinescopeTesting.Api.ApiHelpers.KinescopeApi;
using RestSharp;

namespace KinescopeTesting.Api.ApiHelpers.CommonApiHelpers
{
    public class HtmlApiHelper : BaseKinescopeApiHelper
    {
        public HtmlApiHelper() : base()
        { }

        public async Task<HtmlDocument> GetHtmlAsync(string url)
        {
            var request = new RestRequest(url);

            var response = await SendRequestAsync(request, Method.Get);

            var doc = new HtmlDocument();

            doc.LoadHtml(response.Content);

            return doc;
        }
    }
}
