using KinescopeTesting.Api.ApiHelpers;
using Serilog;

namespace KinescopeTesting.TestData
{
    public static class TestDataManager
    {
        private const string Url = "https://test-videos.co.uk/bigbuckbunny/";
        private const string Domain = "https://test-videos.co.uk";

        private static string VideoLink1Mb1920x1080 { get; set; }
        private static string VideoLink30Mb1920x1080 { get; set; }
        private static string VideoLink1Mb1280x720 { get; set; }
        private static string VideoLink30Mb1280x720 { get; set; }
        private static string VideoLink1Mb640x360 { get; set; }
        private static string VideoLink30Mb640x360 { get; set; }

        public static class VideoTypes
        {
            public const string Mp4H264 = "mp4-h264";
            public const string Mp4H265 = "mp4-h265";
            public const string Mp4Av1 = "mp4-av1";
            public const string WebmVp8 = "webm-vp8";
            public const string WebmVp9 = "webm-vp9";
            public const string WebmAv1 = "webm-av1";
            public const string Mkv = "mkv";
        }

        public static List<string> VideoLinks { get; private set; }

        public static string RandomImageLink = "https://picsum.photos/200";

        public static async Task InitializeAsync(string videoType)
        {
            VideoLinks = new List<string>();

            var videoLinks = await GetVideoLinks(videoType);

            VideoLink1Mb1920x1080 = videoLinks.FirstOrDefault(link => link.Contains("1080") && link.Contains("1MB"));
            if (VideoLink1Mb1920x1080 == null)
            {
                Log.Warning("Не найдено видео с разрешением 1920x1080 и размером 1MB.");
            }

            VideoLink30Mb1920x1080 = videoLinks.FirstOrDefault(link => link.Contains("1080") && link.Contains("20MB"));
            if (VideoLink30Mb1920x1080 == null)
            {
                Log.Warning("Не найдено видео с разрешением 1920x1080 и размером 30MB.");
            }

            VideoLink1Mb1280x720 = videoLinks.FirstOrDefault(link => link.Contains("720") && link.Contains("1MB"));
            if (VideoLink1Mb1280x720 == null)
            {
                Log.Warning("Не найдено видео с разрешением 1280x720 и размером 1MB.");
            }

            VideoLink30Mb1280x720 = videoLinks.FirstOrDefault(link => link.Contains("720") && link.Contains("20MB"));
            if (VideoLink30Mb1280x720 == null)
            {
                Log.Warning("Не найдено видео с разрешением 1280x720 и размером 30MB.");
            }

            VideoLink1Mb640x360 = videoLinks.FirstOrDefault(link => link.Contains("360") && link.Contains("1MB"));
            if (VideoLink1Mb640x360 == null)
            {
                Log.Warning("Не найдено видео с разрешением 640x360 и размером 1MB.");
            }

            VideoLink30Mb640x360 = videoLinks.FirstOrDefault(link => link.Contains("360") && link.Contains("20MB"));
            if (VideoLink30Mb640x360 == null)
            {
                Log.Warning("Не найдено видео с разрешением 640x360 и размером 30MB.");
            }

            var links = new List<string>
            {
                VideoLink1Mb1920x1080,
                VideoLink30Mb1920x1080,
                VideoLink1Mb1280x720,
                VideoLink30Mb1280x720,
                VideoLink1Mb640x360,
                VideoLink30Mb640x360
            };

            links.ForEach(link =>
            {
                if (!string.IsNullOrEmpty(link))
                {
                    Log.Information("Добавлена ссылка на видео: {VideoLink}", link);
                    VideoLinks.Add(link);
                }
            });
        }

        public static async Task<List<string>> GetVideoLinks(string videoType)
        {
            Log.Information("Получение списка ссылок на видео для типа: {VideoType}", videoType);
            var page = await ApiHelpersManager.HtmlApiHelper.GetHtmlAsync($"{Url}{videoType}");

            var videoLinks = page.DocumentNode.SelectNodes("//a[@href]")
                                      .Select(a => a.GetAttributeValue("href", string.Empty))
                                      .Where(href => href.EndsWith(".mp4") || href.EndsWith(".webm") || href.EndsWith(".mkv"))
                                      .Select(href => $"{Domain}{href}")
                                      .ToList();

            Log.Information("Найдено {Count} ссылок на видео для типа {VideoType}", videoLinks.Count, videoType);

            return videoLinks;
        }
    }
}