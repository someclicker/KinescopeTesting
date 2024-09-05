using KinescopeTesting.Api.ApiHelpers;
using KinescopeTesting.Api.ApiTypes;
using KinescopeTesting.Api.ApiTypes.ProjectCreate;
using Serilog;
using System.Net;

namespace KinescopeTesting.Tests.ApiUploadTests
{
    public class BaseApiUploadTest : BaseTest
    {
        protected ProjectCreateRequest Project;
        protected ProjectCreateResponse ProjectResponse;

        [TearDown]
        public async Task TearDown()
        {
            if (Project != null)
            {
                try
                {
                    Log.Debug("Попытка удалить проект с ID: {ProjectId}", ProjectResponse.Data.Id);

                    var deleteProjectResponse = await ApiHelpersManager.ProjectsApiHelper.DeleteProjectAsync(
                        ProjectResponse.Data.Id ?? throw new Exception("Не был передан экземпляр ProjectResponse в параметры"));

                    Log.Information("Проект с ID: {ProjectId} удален, код статуса: {StatusCode}",
                        ProjectResponse.Data.Id, deleteProjectResponse.StatusCode);

                    Assert.AreEqual(HttpStatusCode.OK, deleteProjectResponse.StatusCode, "Соответствие коду удаления проекта ожидаемому");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Ошибка при попытке удалить проект.");

                    throw;
                }
            }
            else
            {
                Log.Warning("Project не был инициализирован. Нечего удалять.");
            }
        }
    }
}