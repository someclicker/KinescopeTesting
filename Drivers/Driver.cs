using KinescopeTesting.Drivers.DriverConfigs;
using Microsoft.Playwright;

namespace KinescopeTesting.Drivers
{
    public sealed class Driver : IAsyncDisposable
    {
        public IPlaywright Playwright { get; private set; }
        public IBrowser Browser { get; private set; }
        public IBrowserContext Context { get; private set; }

        private Driver() { }

        public static async Task<Driver> CreateAsync()
        {
            var instance = new Driver();
            await instance.InitializeAsync();

            return instance;
        }

        private async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            Browser = await Playwright.Chromium.LaunchAsync(DriverConfigurator.GetBrowserLaunchOptions());

            Context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = ViewportSize.NoViewport
            });
        }

        public async Task<IPage> NewPageAsync()
        {
            return await Context.NewPageAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (Context != null)
            {
                await Context.CloseAsync();
                Context = null;
            }
            if (Browser != null)
            {
                await Browser.CloseAsync();
                Browser = null;
            }

            Playwright?.Dispose();
            Playwright = null;
        }
    }
}