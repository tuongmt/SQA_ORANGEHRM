using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OrangeHRM.Pages;
using System.Security.Cryptography;
using System.Xml.Linq;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using By = Selenium.WebDriver.Extensions.By;
using OpenQA.Selenium.Interactions;

namespace OrangeHRM.Tests
{
	internal class LoginTests
	{
		public IWebDriver driver;
		private IJavaScriptExecutor js;

		public LoginTests () {}

		[SetUp]
		public void Setup()
		{
			ChromeOptions options = new ChromeOptions();
			options.AddArgument("--start-maximized");
			ChromeDriverService service = ChromeDriverService.CreateDefaultService("");
			driver = new ChromeDriver(service, options);
		}

		[TearDown]
		public void TearDown()
		{
			driver.Quit();
		}

		[Test, Category("Login")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetValidUserDatasFromExcel")]
		public void ExecLogin_WithValidUser_NavigatesToDashboardPage(string username, string password)
		{
			LoginPage loginPage = new LoginPage(driver, js);

			loginPage.GetAPI();

			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);
		}

		[Test, Category("Login")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetInvalidUserDatasFromExcel")]
		public void ExecLogin_WithInvalidUser_ShowsErrorMessage(string username, string password)
		{
			LoginPage loginPage = new LoginPage(driver, js);

			loginPage.GetAPI();

			loginPage.Login_WithInvalidUser_ShowsErrorMessage(username, password);
		}

		[Test, Category("Login")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetValidUserDatasFromExcel")]
		public void ExecLogout_FromHomePage_RedirectToLogin(string username, string password)
		{
			LoginPage loginPage = new LoginPage(driver, js);

			loginPage.GetAPI();

			// Flow logout
			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);
			loginPage.Logout_FromHomePage_RedirectToLogin(username, password);
		}
	}
}
