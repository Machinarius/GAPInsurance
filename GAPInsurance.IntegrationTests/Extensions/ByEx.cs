using OpenQA.Selenium;

namespace GAPInsurance.IntegrationTests.Extensions {
  public static class ByEx {
    public static By PartialText(string text) {
      return By.XPath($"//*[contains(text(), '{text}')]");
    }
  }
}