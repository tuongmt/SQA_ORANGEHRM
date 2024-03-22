using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OrangeHRM.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeHRM.Tests
{
	internal class MyInfoTests
	{
		private IWebDriver _driver;
		private IJavaScriptExecutor _js;

		public MyInfoTests() { }

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

		[Test]
		[TestCaseSource(typeof(ExcelDataProvider), "GetAddJobTitleDatasFromExcel")]
		public void UseAdminTests(string username, string password, string jobTitle)
		{
			LoginPage loginPage = new LoginPage(_driver, _js);
			MyInfoPage myInfoPage = new MyInfoPage(_driver, _js);

			loginPage.GetAPI();

			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			myInfoPage.Admin_AddJobTitle(jobTitle);
		}
	}
}
