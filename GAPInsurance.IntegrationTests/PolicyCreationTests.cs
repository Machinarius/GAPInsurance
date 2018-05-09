using System;
using GAPInsurance.IntegrationTests.Harness;
using GAPInsurance.IntegrationTests.Pages;
using NFluent;
using OpenQA.Selenium;
using Xunit;

namespace GAPInsurance.IntegrationTests {
  public class PolicyCreationTests : IDisposable {
    private readonly IWebDriver webDriver;

    public PolicyCreationTests() {
      webDriver = TestHarness.CreateWebDriver();
    }

    public void Dispose() {
      webDriver.Close();
      webDriver.Dispose();
    }

    [Fact]
    public void CreatingAPolicyWithDefaultDataMustFail() {
      var loginPage = new LoginPage(webDriver);
      loginPage.Login(TestData.AdminUser.Username, TestData.AdminUser.Password);

      var dashboardPage = new DashboardPage(webDriver);
      dashboardPage.CreatePolicyButton.Click();

      var creationPage = new PolicyCreationPage(webDriver);
      creationPage.FormSubmitButton.Click();

      Check
        .That(creationPage.NameErrorLabel.Text)
        .IsEqualTo("A policy requires a name");

      Check
        .That(creationPage.DescriptionErrorLabel.Text)
        .IsEqualTo("A policy requires a description");

      Check
        .That(creationPage.CoveragesErrorLabel.Text)
        .IsEqualTo("At least one coverage value is needed");

      Check
        .That(creationPage.CoverageLengthErrorLabel.Text)
        .IsEqualTo("Coverage must last for a month at least");

      Check
        .That(creationPage.PremiumValueErrorLabel.Text)
        .IsEqualTo("The insurance premium must be greater than 0");
    }

    [Fact]
    public void CreatingAPolicyWithLowRiskMustShowItInTheDashboard() {
      var loginPage = new LoginPage(webDriver);
      loginPage.Login(TestData.AdminUser.Username, TestData.AdminUser.Password);

      var dashboardPage = new DashboardPage(webDriver);
      dashboardPage.CreatePolicyButton.Click();

      var creationPage = new PolicyCreationPage(webDriver);
      creationPage.PolicyNameField.SendKeys("Test policy");
      creationPage.PolicyDescriptionField.SendKeys("This is a test");
      creationPage.EarthquakeCoverageField.SendKeys("65");
      creationPage.PolicyCoveragePeriodField.SendKeys("5");
      creationPage.PremiumValueField.SendKeys("56");
      creationPage.RiskLevelDropdown.SelectByText("Medium");
      creationPage.FormSubmitButton.Click();

      var policDetailsLink = dashboardPage.GetPolicyDetailsLink("Test Policy");
      Check.That(policDetailsLink).IsNotNull();
      policDetailsLink.Click();

      
    }
  }
}