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

namespace OrangeHRM.Tests
{
	internal class LoginTests
	{
		public IWebDriver driver;
		private IJavaScriptExecutor js;

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

		[Test]
		public void GetAPI()
		{
			string URL = "http://localhost/orangehrm-5.4/orangehrm-5.4/web/index.php/auth/login";
			driver.Navigate().GoToUrl(URL);
			driver.Manage().Window.Size = new System.Drawing.Size(1000, 1200);
			Thread.Sleep(3000);
		}

		[Test, Category("Login")]
		//[TestCase(TestName = "Login with valid user redirects to dashboard page")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetValidUserDatasFromExcel")]
		public void Login_WithValidUser_NavigatesToDashboardPage(string username, string password)
		{
			GetAPI();

			driver.FindElement(By.Name("username")).SendKeys(username);
			driver.FindElement(By.Name("password")).SendKeys(password);
			driver.FindElement(By.CssSelector("button[type='submit']")).Click();

			// Check error message when login fail
			var errorMessage = driver.FindElements(By.CssSelector(".oxd-text.oxd-text--p.oxd-alert-content-text"));
			Assert.That(errorMessage.Count == 0, "Error message is invisible");

			// If login success redirects to homepage and see userdropdown-tab then login success
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-userdropdown-tab")).Count > 0);
			}
			var actual = driver.FindElements(By.CssSelector(".oxd-userdropdown-tab"));
			Assert.That(actual.Count > 0, "Login success");
			//Write result to excel (FileName.xlsx, "WorkSheetName", "Result", ColumnToWriteResult)
			if(actual.Count > 0) 
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "ValidUser", "Pass", 4);
			else 
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "ValidUser", "Fail", 4);

			//When not run other function then turn on it
			//driver.Close(); 
		}

		[Test, Category("Login")]
		//[TestCase(TestName = "Login with a invalid user shows error message")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetInvalidUserDatasFromExcel")]
		public void Login_WithInvalidUser_ShowsErrorMessage(string username, string password)
		{
			GetAPI();

			driver.FindElement(By.Name("username")).SendKeys(username);
			driver.FindElement(By.Name("password")).SendKeys(password);
			driver.FindElement(By.CssSelector("button[type='submit']")).Click();
			Thread.Sleep(3000);
			
			var errorMessage = driver.FindElements(By.CssSelector(".oxd-text.oxd-text--p.oxd-alert-content-text"));
			Assert.That(errorMessage.Count > 0, Is.True, "Error message is visible");
			
			
			var actual = driver.FindElements(By.CssSelector(".oxd-userdropdown-name"));
			Assert.That(actual.Count == 0, "Login fail");

			if (actual.Count == 0)
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "InvalidUser", "Pass", 4);
			else
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "InvalidUser", "Fail", 4);
			driver.Close();
		}

		[Test, Category("Login")]
		//[TestCase(TestName = "Logout redirects to login")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetValidUserDatasFromExcel")]
		public void Logout_FromHomePage_RedirectToLogin(string username, string password)
		{
			Login_WithValidUser_NavigatesToDashboardPage(username, password);

			driver.FindElement(By.CssSelector(".oxd-userdropdown-tab")).Click();
			Thread.Sleep(2000);
			driver.FindElement(By.LinkText("Logout")).Click();
			Thread.Sleep(2000);
			var actual = driver.FindElements(By.CssSelector(".oxd-button"));
			Console.WriteLine(actual);
			Assert.That(actual.Count > 0, "Logout success");
			driver.Close();
		}
	}
}
