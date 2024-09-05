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
        [Description($"���������� ����. ������������������� �������� ����� ������� {TestDataManager.VideoTypes.Mp4H264} ��������� ��������")]
        public async Task RunUploadVideoApiPositiveTest(string videoLink)
        {
            Log.Information("������ ����� �������� ����� �� API: {VideoLink}", videoLink);

            Project = new ProjectCreateRequest
            {
                Name = new Faker().Lorem.Word() + new Faker().Lorem.Word(),
                PrivacyType = TestParameters.Custom,
                PrivacyDomains = new List<string> { TestParameters.CustomDomain },
                CatalogType = TestParameters.CatalogType
            };

            Log.Debug("�������� ������� �� �������� ������� � ������: {ProjectName}", Project.Name);

            ProjectResponse = await ApiHelpersManager.ProjectsApiHelper.CreateProjectAsync(Project);

            Assert.IsNotNull(ProjectResponse.Data.Id, "������ ������ ���� ������ � ����������� ���������� ID");

            Log.Information("������ ������ � ID: {ProjectId}", ProjectResponse.Data.Id);

            Log.Debug("�������� ����� �� ������: {VideoLink}", videoLink);

            var videoUploadResponse = await ApiHelpersManager.VideoDownloadApiHelper
                .UploadVideoByLinkAsync(videoLink, TestParameters.FileTitle, ProjectResponse.Data.Id);

            Assert.IsNotNull(videoUploadResponse.ApiTypeData.Id,
                "����� �� �������� ����� ������ ��������� ���������� ID ������������ �����");

            Log.Information("����� ������� ��������� � ID: {VideoId}", videoUploadResponse.ApiTypeData.Id);

            Assert.AreEqual(TestParameters.FileTitle, videoUploadResponse.ApiTypeData.Title,
                "�������� ������������ ����� ������ ��������� � ��������� � �������");

            Log.Information("�������� ������������ ����� ������������� ����������: {VideoTitle}",
                TestParameters.FileTitle);

            await CommonSteps.Authorization(Page, Environment.GetEnvironmentVariable("LOGIN"),
                Environment.GetEnvironmentVariable("PASSWORD"));


            var workAreaPage = new WorkAreaPage(Page);

            Assert.IsTrue(await workAreaPage.IsPageDisplayed(), "������� �������� ������ ������������ ����� �����");

            Log.Information("������� �������� ������������ ��������� ����� �����.");

            Assert.IsTrue(await workAreaPage.IsProjectInMenuDisplayed(ProjectResponse.Data.Name),
                "��������� ������ ������ ������������ � ���� ��������");

            Log.Information("������ {ProjectName} ������������ � ����.", ProjectResponse.Data.Name);

            await workAreaPage.ClickProject(Project.Name);

            Log.Debug("������� � �������: {ProjectName}", Project.Name);

            Assert.IsTrue(await workAreaPage.IsVideoUploaded(videoUploadResponse.ApiTypeData.Title),
                "����������� ����� ������ ������������ � �������");

            Log.Information("����������� ����� {VideoTitle} ������������ � �������.",
                videoUploadResponse.ApiTypeData.Title);

            workAreaPage.ClickVideo(videoUploadResponse.ApiTypeData.Title);

            Log.Debug("������� � �����: {VideoTitle}", videoUploadResponse.ApiTypeData.Title);

            var detailsPage = new VideoDetailsPage(Page);

            Assert.IsTrue(await detailsPage.IsVideoAviliableToPlay(),
                $"����� ������ ���� �������� ��� ��������������� �� �������� ������� {videoLink}");

            Log.Information("����� �������� ��� ���������������.");

            Assert.IsFalse(await detailsPage.IsErrorDisplayed(),
                "������ �� ������ ������������ �� �������� �������");

            Log.Information("������ �� �������� ������� �� ����������.");
        }
    }
}
