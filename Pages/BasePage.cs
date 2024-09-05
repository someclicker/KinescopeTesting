using Microsoft.Playwright;
using Serilog;

namespace KinescopeTesting.Pages
{
    public class BasePage
    {
        private const int WaitingTimeout = 40000;

        protected IPage Page;
        protected ILocator UniqueElementLocator;
        protected string Url;
        protected string Name;

        public BasePage(IPage page, string name, string url, string uniqueElementLocator)
        {
            Page = page;
            Name = name;
            Url = url;
            UniqueElementLocator = Page.Locator(uniqueElementLocator);

            Log.Information("Инициализация страницы {PageName}", Name);
        }

        public async Task GoToPage()
        {
            Log.Information("Переход на страницу: {PageName}", Name);
            await Page.GotoAsync(Url);
            await WaitForPageLoad();
            Log.Information("Переход на страницу {PageName} завершен", Name);
        }

        public async Task<bool> IsPageDisplayed()
        {
            bool isVisible = await IsElementVisible(UniqueElementLocator);
            Log.Information("Страница {PageName} отображается: {IsDisplayed}", Name, isVisible);

            return isVisible;
        }

        public async Task ClickElement(ILocator locator)
        {
            Log.Information("Клик на элемент на странице {PageName}", Name);

            await locator.ClickAsync();

            Log.Information("Элемент на странице {PageName} кликнут", Name);
        }

        public async Task TypeText(ILocator locator, string text)
        {
            Log.Information("Ввод текста в элемент на странице {PageName}", Name);

            await locator.FillAsync(text);

            Log.Information("Текст введен в элемент на странице {PageName}", Name);
        }

        public async Task<bool> IsElementVisible(ILocator locator, int timeout = WaitingTimeout)
        {
            try
            {
                Log.Information("Проверка видимости элемента на странице {PageName}", Name);

                await locator.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = timeout
                });

                bool isVisible = await locator.IsVisibleAsync();

                Log.Information("Видимость элемента на странице {PageName}: {IsVisible}", Name, isVisible);

                return isVisible;
            }
            catch (TimeoutException ex)
            {
                Log.Warning(ex, "TimeoutException при проверке видимости элемента на странице {PageName}", Name);

                return false;
            }
        }

        public async Task<bool> IsElementEnabled(ILocator locator, int timeout = WaitingTimeout)
        {
            try
            {
                Log.Information("Проверка доступности элемента на странице {PageName}", Name);

                await locator.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Attached,
                    Timeout = timeout
                });

                bool isEnabled = await locator.IsEnabledAsync();

                Log.Information("Доступность элемента на странице {PageName}: {IsEnabled}", Name, isEnabled);

                return isEnabled;
            }
            catch (TimeoutException ex)
            {
                Log.Warning(ex, "TimeoutException при проверке доступности элемента на странице {PageName}", Name);

                return false;
            }
        }

        public async Task WaitForPageLoad(int timeout = WaitingTimeout)
        {
            try
            {
                Log.Information("Ожидание загрузки страницы {PageName}", Name);

                await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions
                {
                    Timeout = timeout
                });

                Log.Information("Загрузка страницы {PageName} завершена", Name);
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex, "TimeoutException при ожидании загрузки страницы {PageName}", Name);

                throw;
            }
        }
    }
}