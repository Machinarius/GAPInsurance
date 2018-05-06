using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace GAPInsurance.IntegrationTests.Harness {
  public static class TestHarness {
    public static IWebDriver CreateWebDriver() {
      var driverPath = System.IO.Path.GetFullPath(".");
      var driver = new FirefoxDriver(driverPath);
      driver.Navigate().GoToUrl("http://webclient");

      return driver;
    }
  }
}