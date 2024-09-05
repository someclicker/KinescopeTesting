using Bogus;
using KinescopeTesting.Api.ApiHelpers;
using KinescopeTesting.Api.ApiTypes.ProjectCreate;
using KinescopeTesting.Pages;
using KinescopeTesting.TestData;
using KinescopeTesting.Tests.Steps;
using Serilog;

namespace KinescopeTesting.Tests.ApiUploadTests.Negative
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class UploadImageApiNegativeTest : BaseApiUploadTest
    {
        public static class TestParameters
        {
            public static readonly string FileTitle = new Faker().Lorem.Word() + new Faker().Lorem.Word();
            public static readonly string Custom = "custom";
            public static readonly string CustomDomain = "custom.domain.com";
            public static readonly string CatalogType = "vod";
        }

        [Test]
        [Description("Негативный тест. По API передается изображение")]
        public async Task RunUploadVideoApiNegativeTest()
        {
            Log.Information("Запуск негативного теста загрузки изображения по API.");

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

            Log.Debug("Попытка загрузить изображение по ссылке: {ImageLink}", TestDataManager.RandomImageLink);

            var videoUploadResponse = await ApiHelpersManager.VideoDownloadApiHelper
                .UploadVideoByLinkAsync(TestDataManager.RandomImageLink, TestParameters.FileTitle, ProjectResponse.Data.Id);

            Assert.IsNotNull(videoUploadResponse.ApiTypeData.Id, "Ответ на загрузку видео должен содержать уникальный ID загруженного файла");

            Log.Information("Файл успешно загружен с ID: {FileId}", videoUploadResponse.ApiTypeData.Id);

            Assert.AreEqual(TestParameters.FileTitle, videoUploadResponse.ApiTypeData.Title, "Название загруженного файла должно совпадать с указанным в запросе");

            Log.Information("Название загруженного файла соответствует ожидаемому: {FileTitle}", TestParameters.FileTitle);

            await CommonSteps.Authorization(Page, Environment.GetEnvironmentVariable("LOGIN"), Environment.GetEnvironmentVariable("PASSWORD"));

            await CommonSteps.Authorization(Page, "totocrew@rambler.ru", "Gosha158!");

            var workAreaPage = new WorkAreaPage(Page);
            Assert.IsTrue(await workAreaPage.IsPageDisplayed(), "Рабочая страница должна отображаться после входа");

            Log.Information("Рабочая страница отображается корректно после входа.");

            Assert.IsTrue(await workAreaPage.IsProjectInMenuDisplayed(ProjectResponse.Data.Name), "Созданный проект должен отображаться в меню проектов");

            Log.Information("Проект {ProjectName} отображается в меню.", ProjectResponse.Data.Name);

            await workAreaPage.ClickProject(Project.Name);

            Log.Debug("Переход к проекту: {ProjectName}", Project.Name);

            Assert.IsTrue(await workAreaPage.IsVideoUploaded(videoUploadResponse.ApiTypeData.Title), "Загруженный файл должен отображаться в проекте");

            Log.Information("Загруженный файл {FileTitle} отображается в проекте.", videoUploadResponse.ApiTypeData.Title);

            workAreaPage.ClickVideo(videoUploadResponse.ApiTypeData.Title);

            Log.Debug("Переход к файлу: {FileTitle}", videoUploadResponse.ApiTypeData.Title);

            var detailsPage = new VideoDetailsPage(Page);

            Assert.IsTrue(await detailsPage.IsErrorDisplayed(), "Ошибка должна отображаться на странице деталей");

            Log.Information("Ошибка отображается на странице деталей.");

            Assert.IsFalse(await detailsPage.IsVideoAviliableToPlay(), "Плеер недоступен");

            Log.Information("Плеер недоступен, как и ожидалось.");
        }
    }
}