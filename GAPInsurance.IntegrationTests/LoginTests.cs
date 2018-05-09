using System;
using GAPInsurance.IntegrationTests.Extensions;
using GAPInsurance.IntegrationTests.Harness;
using GAPInsurance.IntegrationTests.Pages;
using NFluent;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace GAPInsurance.IntegrationTests {
  public class LoginTests : IDisposable {
    private readonly IWebDriver webDriver;

    public LoginTests() {
      webDriver = TestHarness.CreateWebDriver();
    }

    public void Dispose() {
      webDriver.Close();
      webDriver.Dispose();
    }

    [Fact]
    public void TheAppMustShowALoginScreenByDefault() {
      var loginPage = new LoginPage(webDriver);
      Check.That(loginPage.UsernameField).IsNotNull();
      Check.That(loginPage.PasswordField).IsNotNull();
    }

    [Fact]
    public void TheAppMustShowTheDashboardWhenAnAdminLogsIn() {
      var loginPage = new LoginPage(webDriver);
      loginPage
        .UsernameField
        .SendKeys(TestData.AdminUser.Username);

      loginPage
        .PasswordField
        .SendKeys(TestData.AdminUser.Password);

      loginPage.LoginButton.Click();

      var authMenu = new AuthMenu(webDriver);
      var loginWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
      loginWait.Until(_ => authMenu.NameLabel != null);

      Check.That(authMenu.NameLabel.Text).IsEqualTo(TestData.CustomerServiceUser.Name);
      Check.That(authMenu.RoleLabel.Text).IsEqualTo("Administrator");

      webDriver.FindElement(ByEx.PartialText("Clients"));
      webDriver.FindElement(ByEx.PartialText("Policies"));
    }

    [Fact]
    public void TheAppMustShowTheDashboardWhenAServiceAgentLogsIn() {
      var loginPage = new LoginPage(webDriver);
      loginPage
        .UsernameField
        .SendKeys(TestData.CustomerServiceUser.Username);

      loginPage
        .PasswordField
        .SendKeys(TestData.CustomerServiceUser.Password);

      loginPage.LoginButton.Click();

      var authMenu = new AuthMenu(webDriver);
      var loginWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
      loginWait.Until(_ => authMenu.NameLabel != null);

      Check.That(authMenu.NameLabel.Text).IsEqualTo(TestData.CustomerServiceUser.Name);
      Check.That(authMenu.RoleLabel.Text).IsEqualTo("Customer Service Agent");

      webDriver.FindElement(ByEx.PartialText("Clients"));
      webDriver.FindElement(ByEx.PartialText("Policies"));
    }

    [Fact]
    public void TheAppMustAllowTheUserToLogOut() {
      var loginPage = new LoginPage(webDriver);
      loginPage
        .UsernameField
        .SendKeys(TestData.CustomerServiceUser.Username);

      loginPage
        .PasswordField
        .SendKeys(TestData.CustomerServiceUser.Password);

      loginPage.LoginButton.Click();

      var authMenu = new AuthMenu(webDriver);
      var loginWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
      loginWait.Until(_ => authMenu.SignOutButton);

      authMenu.SignOutButton.Click();
      Check.That(loginPage.UsernameField).IsNotNull();
      Check.That(loginPage.PasswordField).IsNotNull();
    }
  }
}