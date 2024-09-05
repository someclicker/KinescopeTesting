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
        [Description("���������� ����. �� API ���������� �����������")]
        public async Task RunUploadVideoApiNegativeTest()
        {
            Log.Information("������ ����������� ����� �������� ����������� �� API.");

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

            Log.Debug("������� ��������� ����������� �� ������: {ImageLink}", TestDataManager.RandomImageLink);

            var videoUploadResponse = await ApiHelpersManager.VideoDownloadApiHelper
                .UploadVideoByLinkAsync(TestDataManager.RandomImageLink, TestParameters.FileTitle, ProjectResponse.Data.Id);

            Assert.IsNotNull(videoUploadResponse.ApiTypeData.Id, "����� �� �������� ����� ������ ��������� ���������� ID ������������ �����");

            Log.Information("���� ������� �������� � ID: {FileId}", videoUploadResponse.ApiTypeData.Id);

            Assert.AreEqual(TestParameters.FileTitle, videoUploadResponse.ApiTypeData.Title, "�������� ������������ ����� ������ ��������� � ��������� � �������");

            Log.Information("�������� ������������ ����� ������������� ����������: {FileTitle}", TestParameters.FileTitle);

            await CommonSteps.Authorization(Page, Environment.GetEnvironmentVariable("LOGIN"), Environment.GetEnvironmentVariable("PASSWORD"));

            await CommonSteps.Authorization(Page, "totocrew@rambler.ru", "Gosha158!");

            var workAreaPage = new WorkAreaPage(Page);
            Assert.IsTrue(await workAreaPage.IsPageDisplayed(), "������� �������� ������ ������������ ����� �����");

            Log.Information("������� �������� ������������ ��������� ����� �����.");

            Assert.IsTrue(await workAreaPage.IsProjectInMenuDisplayed(ProjectResponse.Data.Name), "��������� ������ ������ ������������ � ���� ��������");

            Log.Information("������ {ProjectName} ������������ � ����.", ProjectResponse.Data.Name);

            await workAreaPage.ClickProject(Project.Name);

            Log.Debug("������� � �������: {ProjectName}", Project.Name);

            Assert.IsTrue(await workAreaPage.IsVideoUploaded(videoUploadResponse.ApiTypeData.Title), "����������� ���� ������ ������������ � �������");

            Log.Information("����������� ���� {FileTitle} ������������ � �������.", videoUploadResponse.ApiTypeData.Title);

            workAreaPage.ClickVideo(videoUploadResponse.ApiTypeData.Title);

            Log.Debug("������� � �����: {FileTitle}", videoUploadResponse.ApiTypeData.Title);

            var detailsPage = new VideoDetailsPage(Page);

            Assert.IsTrue(await detailsPage.IsErrorDisplayed(), "������ ������ ������������ �� �������� �������");

            Log.Information("������ ������������ �� �������� �������.");

            Assert.IsFalse(await detailsPage.IsVideoAviliableToPlay(), "����� ����������");

            Log.Information("����� ����������, ��� � ���������.");
        }
    }
}