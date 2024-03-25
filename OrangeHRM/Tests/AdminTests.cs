using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using By = Selenium.WebDriver.Extensions.By;
using System.Threading;
using System.Security.Cryptography;
using OrangeHRM.Pages;
using OpenQA.Selenium.Chrome;

namespace OrangeHRM.Tests
{
	internal class AdminTests
	{
		private IWebDriver _driver;
		private IJavaScriptExecutor _js;

		public AdminTests() {}

		[SetUp]
		public void Setup()
		{
			ChromeOptions options = new ChromeOptions();
			options.AddArgument("--start-maximized");
			ChromeDriverService service = ChromeDriverService.CreateDefaultService("");
			_driver = new ChromeDriver(service, options);
		}

		[TearDown]
		public void TearDown()
		{
			_driver.Quit();
		}

		[Test, Category("Admin")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetAddJobTitleDatasFromExcel")]
		public void ExecAdmin_AddJobTitle(string username, string password, string jobTitle)
		{
			LoginPage loginPage = new LoginPage(_driver, _js);
			AdminPage adminPage = new AdminPage(_driver, _js);

			loginPage.GetAPI();
			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			adminPage.FlowEnteringAdmin();
			adminPage.Admin_AddJobTitle(jobTitle);
		}
	}
}
