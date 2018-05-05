using System;
using OpenQA.Selenium;
using Xunit;

namespace GAPInsurance.IntegrationTests {
  public class GoogleTests : IDisposable {
    private readonly IWebDriver webDriver;

    public GoogleTests() {
      webDriver = TestHarness.CreateWebDriver();
    }

    [Fact]
    public void TestGoogleDownload() {
      webDriver.Navigate().GoToUrl("https://google.com");
      webDriver.FindElement(By.PartialLinkText("Gmail"));
    }

    public void Dispose() {
      webDriver.Close();
      webDriver.Dispose();
    }
  }
}