using KinescopeTesting.Drivers;
using Microsoft.Playwright;
using Serilog;

namespace KinescopeTesting.Tests
{
    public class BaseTest : PageTest
    {
        protected IPage Page;
        protected Driver Browser;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
                .WriteTo.Console()
                 .WriteTo.File("Logs/logfile.txt", rollingInterval: RollingInterval.Day)
                  .CreateLogger();
        }

        [SetUp]
        public async Task SetUp()
        {
            Log.Debug("Инициализация браузера...");
            Browser = await Driver.CreateAsync();
            Log.Information("Браузер инициализирован.");

            Page = await Browser.NewPageAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            if (Page != null)
            {
                Log.Debug("Закрытие страницы...");
                await Page.CloseAsync();
                Page = null;
                Log.Information("Страница закрыта.");
            }

            if (Browser != null)
            {
                Log.Debug("Закрытие браузера...");
                await Browser.DisposeAsync();
                Browser = null;
                Log.Information("Браузер закрыт.");

                Log.CloseAndFlush();
            }
        }
    }
}