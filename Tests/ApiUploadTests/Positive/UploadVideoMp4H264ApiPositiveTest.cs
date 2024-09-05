using Bogus;
using KinescopeTesting.Api.ApiHelpers;
using KinescopeTesting.Api.ApiTypes.ProjectCreate;
using KinescopeTesting.Pages;
using KinescopeTesting.TestData;
using KinescopeTesting.Tests.Steps;
using Serilog;

namespace KinescopeTesting.Tests.ApiUploadTests.Positive
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class UploadVideoMp4H264ApiPositiveTest : BaseApiUploadTest
    {
        public static class TestParameters
        {
            public static readonly string FileTitle = new Faker().Lorem.Word() + new Faker().Lorem.Word();
            public static readonly string Custom = "custom";
            public static readonly string CustomDomain = "custom.domain.com";
            public static readonly string CatalogType = "vod";

            public static async Task<IEnumerable<string>> GetVideoLinks()
            {
                await TestDataManager.InitializeAsync(TestDataManager.VideoTypes.Mp4H264);
                return TestDataManager.VideoLinks;
            }
        }

        [Test, TestCaseSource(typeof(TestParameters), nameof(TestParameters.GetVideoLinks))]
        [Description($"Позитивный тест. Параметризированная передача видео формата {TestDataManager.VideoTypes.Mp4H264} различных размеров")]
        public async Task RunUploadVideoApiPositiveTest(string videoLink)
        {
            Log.Information("Запуск теста загрузки видео по API: {VideoLink}", videoLink);

            Project = new ProjectCreateRequest
            {
                Name = new Faker().Lorem.Word() + new Faker().Lorem.Word(),
                PrivacyType = TestParameters.Custom,
                PrivacyDomains = new List<string> { TestParameters.CustomDomain },
                CatalogType = TestParameters.CatalogType
            };

            Log.Debug("Отправка запроса на создание проекта с именем: {ProjectName}", Project.Name);

            ProjectResponse = await ApiHelpersManager.ProjectsApiHelper.CreateProjectAsync(Project);

            Assert.IsNotNull(ProjectResponse.Data.Id, "Проект должен быть создан с присвоенным уникальным ID");

            Log.Information("Проект создан с ID: {ProjectId}", ProjectResponse.Data.Id);

            Log.Debug("Загрузка видео по ссылке: {VideoLink}", videoLink);

            var videoUploadResponse = await ApiHelpersManager.VideoDownloadApiHelper
                .UploadVideoByLinkAsync(videoLink, TestParameters.FileTitle, ProjectResponse.Data.Id);

            Assert.IsNotNull(videoUploadResponse.ApiTypeData.Id,
                "Ответ на загрузку видео должен содержать уникальный ID загруженного файла");

            Log.Information("Видео успешно загружено с ID: {VideoId}", videoUploadResponse.ApiTypeData.Id);

            Assert.AreEqual(TestParameters.FileTitle, videoUploadResponse.ApiTypeData.Title,
                "Название загруженного файла должно совпадать с указанным в запросе");

            Log.Information("Название загруженного видео соответствует ожидаемому: {VideoTitle}",
                TestParameters.FileTitle);

            await CommonSteps.Authorization(Page, Environment.GetEnvironmentVariable("LOGIN"),
                Environment.GetEnvironmentVariable("PASSWORD"));


            var workAreaPage = new WorkAreaPage(Page);

            Assert.IsTrue(await workAreaPage.IsPageDisplayed(), "Рабочая страница должна отображаться после входа");

            Log.Information("Рабочая страница отображается корректно после входа.");

            Assert.IsTrue(await workAreaPage.IsProjectInMenuDisplayed(ProjectResponse.Data.Name),
                "Созданный проект должен отображаться в меню проектов");

            Log.Information("Проект {ProjectName} отображается в меню.", ProjectResponse.Data.Name);

            await workAreaPage.ClickProject(Project.Name);

            Log.Debug("Переход к проекту: {ProjectName}", Project.Name);

            Assert.IsTrue(await workAreaPage.IsVideoUploaded(videoUploadResponse.ApiTypeData.Title),
                "Загруженное видео должно отображаться в проекте");

            Log.Information("Загруженное видео {VideoTitle} отображается в проекте.",
                videoUploadResponse.ApiTypeData.Title);

            workAreaPage.ClickVideo(videoUploadResponse.ApiTypeData.Title);

            Log.Debug("Переход к видео: {VideoTitle}", videoUploadResponse.ApiTypeData.Title);

            var detailsPage = new VideoDetailsPage(Page);

            Assert.IsTrue(await detailsPage.IsVideoAviliableToPlay(),
                $"Видео должно быть доступно для воспроизведения на странице деталей {videoLink}");

            Log.Information("Видео доступно для воспроизведения.");

            Assert.IsFalse(await detailsPage.IsErrorDisplayed(),
                "Ошибка не должна отображаться на странице деталей");

            Log.Information("Ошибок на странице деталей не обнаружено.");
        }
    }
}
