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

		[Test, Category("MyInfo")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetChangeProfilePictureDatasFromExcel")]
		public void ExecMyInfo_ChangeProfilePicture(string username, string password, string path)
		{
			LoginPage loginPage = new LoginPage(_driver, _js);
			MyInfoPage myInfoPage = new MyInfoPage(_driver, _js);

			loginPage.GetAPI();

			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			myInfoPage.FlowEnteringMyInfo();
			myInfoPage.MyInfo_ChangeProfilePicture(path);
		}

		[Test, Category("MyInfo")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetPersonalDetailsDatasFromExcel")]
		public void ExecMyInfo_EditPersonalDetails(string username, string password, string firstName, string middleName,
			string lastName, string employeeId, string otherId, string licenseNumber, string licenseExpiryDate, string nationality,
			string maritalStatus, string dob, string gender)
		{
			LoginPage loginPage = new LoginPage(_driver, _js);
			MyInfoPage myInfoPage = new MyInfoPage(_driver, _js);

			loginPage.GetAPI();

			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			myInfoPage.FlowEnteringMyInfo();
			myInfoPage.MyInfo_EditPersonalDetails(firstName, middleName, lastName, employeeId, otherId, licenseNumber,
				licenseExpiryDate, nationality, maritalStatus, dob, gender);
		}
	}
}
