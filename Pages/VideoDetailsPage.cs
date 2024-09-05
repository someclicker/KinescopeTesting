using Microsoft.Playwright;

namespace KinescopeTesting.Pages
{
    public class VideoDetailsPage : BasePage
    {
        private ILocator GetProjectInMenuLocator(string projectName) => Page.Locator($"//*[contains(@href, 'video')]//*[contains(text(),'{projectName}')]");
        private ILocator VideoPlayer;
        private ILocator ErrorMessage;
        private ILocator AssetsLink;

        public VideoDetailsPage(IPage page)
            : base(page, "Страница видео (детальная)", "https://app.kinescope.io/", "//*[contains(text(),'Постер')]")
        {
            VideoPlayer = page.Locator("//*[@id='kinescope-player']");
            ErrorMessage = page.Locator("//*[contains(text(),'Ошибка')]");
            AssetsLink = page.Locator("//*[contains(text(),'Файлы')]");
        }

        public async Task ClickProject(string projectName) => await ClickElement(GetProjectInMenuLocator(projectName));

        public async Task ClickAssetsLink() => await ClickElement(AssetsLink);

        public async Task<bool> IsVideoAviliableToPlay() => await IsElementEnabled(VideoPlayer);

        public async Task<bool> IsErrorDisplayed() => await IsElementVisible(ErrorMessage);
    }
}