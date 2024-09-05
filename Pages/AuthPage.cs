using Microsoft.Playwright;

namespace KinescopeTesting.Pages
{
    public class AuthPage : BasePage
    {
        private ILocator EmailField;
        private ILocator PasswordField;
        private ILocator LoginButton;

        public AuthPage(IPage page)
             : base(page, "Страница авторизации", "https://app.kinescope.io/", "//*[text()='Зарегистрироваться']")
        {
            EmailField = Page.Locator("//*[@placeholder='Почта']");
            PasswordField = Page.Locator("//*[@placeholder='Пароль']");
            LoginButton = Page.Locator("//button[@type='submit']");
        }
        public async Task Login(string username, string password)
        {
            await TypeText(EmailField, username);
            await TypeText(PasswordField, password);
            await ClickElement(LoginButton);
        }
    }
}