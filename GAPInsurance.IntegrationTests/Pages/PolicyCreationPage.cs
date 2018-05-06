using System;
using OpenQA.Selenium;

namespace GAPInsurance.IntegrationTests.Pages {
  public class PolicyCreationPage {
    private readonly IWebDriver webDriver;

    public PolicyCreationPage(IWebDriver webDriver) {
      this.webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
    }

    public IWebElement FormSubmitButton => webDriver.FindElement(By.Id("createPolicyButton"));
    public IWebElement NameErrorLabel => webDriver.FindElement(By.Id("nameErrorLabel"));
    public IWebElement DescriptionErrorLabel => webDriver.FindElement(By.Id("descriptionErrorLabel"));
    public IWebElement CoveragesErrorLabel => webDriver.FindElement(By.Id("coveragesErrorLabel"));
    public IWebElement CoverageLengthErrorLabel => webDriver.FindElement(By.Id("coverageLengthErrorLabel"));
    public IWebElement PremiumValueErrorLabel => webDriver.FindElement(By.Id("premiumValueErrorLabel"));
  }
}