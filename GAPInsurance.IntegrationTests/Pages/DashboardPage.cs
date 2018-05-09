using System;
using OpenQA.Selenium;

namespace GAPInsurance.IntegrationTests.Pages {
  public class DashboardPage {
    private readonly IWebDriver webDriver;

    public DashboardPage(IWebDriver webDriver) {
      this.webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
    }

    public IWebElement CreatePolicyButton => webDriver.FindElement(By.Id("createPolicyButton"));

    public IWebElement GetPolicyDetailsLink(string policyName) {
      return webDriver.FindElement(By.LinkText(policyName));
    }
  }
}