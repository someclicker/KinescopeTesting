using KinescopeTesting.Pages;
using Microsoft.Playwright;
using Serilog;

namespace KinescopeTesting.Tests.Steps
{
    public static class CommonSteps
    {
        public static async Task Authorization(IPage page, string login, string password)
        {
            var authPage = new AuthPage(page);

            await authPage.GoToPage();

            Assert.IsTrue(await authPage.IsPageDisplayed(), "Страница авторизации должна отображаться");
            Log.Information("Страница авторизации отображается корректно.");

            await authPage.Login(login, password);
            Log.Information("Вход выполнен успешно.");
        }
    }
}
