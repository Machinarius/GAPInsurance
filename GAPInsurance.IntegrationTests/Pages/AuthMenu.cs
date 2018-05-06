using System;
using OpenQA.Selenium;

namespace GAPInsurance.IntegrationTests.Pages {
  public class AuthMenu {
    private readonly IWebDriver webDriver;

    public AuthMenu(IWebDriver webDriver) {
      this.webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
    }

    public IWebElement RoleLabel => webDriver.FindElement(By.Id("roleLabel"));
    public IWebElement NameLabel => webDriver.FindElement(By.Id("nameLabel"));
    public IWebElement SignOutButton => webDriver.FindElement(By.Id("SignOutButton"));
  }
}