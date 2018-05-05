using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace GAPInsurance.IntegrationTests {
  public static class TestHarness {
    public static IWebDriver CreateWebDriver() {
      var completePath = System.IO.Path.GetFullPath(".");
      var driver = new FirefoxDriver(completePath);
      return driver;
    }
  }
}