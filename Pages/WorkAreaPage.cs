using Microsoft.Playwright;

namespace KinescopeTesting.Pages
{
    public class WorkAreaPage : BasePage
    {
        private ILocator GetProjectInMenuLocator(string projectName) => Page.Locator($"//*[contains(@href, 'video')]//*[contains(text(),'{projectName}')]");

        private ILocator GetVideoByTitle(string title) => Page.Locator($"//*[contains(@title,'{title}')]");

        public WorkAreaPage(IPage page)
            : base(page, "Страница 'Рабочая область'", "https://app.kinescope.io/", "//*[text()='Каталог']")
        { }

        public async Task<bool> IsVideoUploaded(string title) => await IsElementVisible(GetVideoByTitle(title));

        public async Task ClickVideo(string title) => await GetVideoByTitle(title).DblClickAsync();

        public async Task<bool> IsProjectInMenuDisplayed(string projectName) => await IsElementVisible(GetProjectInMenuLocator(projectName));

        public async Task ClickProject(string projectName) => await ClickElement(GetProjectInMenuLocator(projectName));
    }
}