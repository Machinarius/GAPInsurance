using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GAPInsurance.IntegrationTests.Pages {
  public class LoginPage {
    private readonly IWebDriver webDriver;

    public LoginPage(IWebDriver webDriver) {
      this.webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
    }

    public IWebElement UsernameField => webDriver.FindElement(By.Id("usernameField"));
    public IWebElement PasswordField => webDriver.FindElement(By.Id("passwordField"));
    public IWebElement LoginButton => webDriver.FindElement(By.Id("loginButton"));

    internal void Login(string username, string password) {
      UsernameField.SendKeys(username);
      PasswordField.SendKeys(password);
      
      var authMenu = new AuthMenu(webDriver);
      var loginWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
      loginWait.Until(_ => authMenu.RoleLabel);
    }
  }
}