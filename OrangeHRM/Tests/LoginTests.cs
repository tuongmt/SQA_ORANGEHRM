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
			try
			{
				string URL = "http://localhost/orangehrm-5.4/orangehrm-5.4/web/index.php/auth/login";
				driver.Navigate().GoToUrl(URL);
				driver.Manage().Window.Position = new System.Drawing.Point(0, 0);
				driver.Manage().Window.Size = new System.Drawing.Size(1000, 825);
				Thread.Sleep(3000);
				//driver.Close();
			}catch(Exception ex)
			{
				Console.WriteLine("Error from GetAPI " + ex.ToString());
			}
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

			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-alert-content.oxd-alert-content--error > p")).Count == 0);
			}
			try
			{
				// Check error message when login fail
				if (driver.FindElement(By.JQuerySelector(".oxd-alert-content.oxd-alert-content--error > p")).Displayed)
				{
					ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "ValidUser", "Fail (Invalid credentials)", 4);
					driver.Close();
					Assert.Fail("Username or password is wrong, check again");
				}
			}
			catch (NoSuchElementException) { }

			// If login success redirects to homepage and see userdropdown-tab then login success
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-sidepanel-body > ul > li:nth-child(8)")).Count > 0);
			}

			// Check visible in Dashboard Page
			if (driver.FindElements(By.CssSelector(".oxd-sidepanel-body > ul > li:nth-child(8)")).Count == 0)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "ValidUser", "Fail (Not redirect to dashboard page)", 4);
				driver.Close();
				Assert.Pass("Test Login with valid user fail because not redirect to dashboard page.");
				
			}

			//When not run other function then turn on it
			//ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "ValidUser", "Pass", 4);
			//driver.Close();
			//Assert.Pass("Test Login with valid user success");
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

			// Wait Error message and check invalid credentials
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-alert-content.oxd-alert-content--error > p")).Count > 0);
				}
			}catch(WebDriverTimeoutException) {
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "InvalidUser", "Fail (It's valid credentials)", 4);
				driver.Close();
				Assert.Fail("It's valid credentials");
			}
			catch (NoSuchElementException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "InvalidUser", "Fail (It's valid credentials)", 4);
				driver.Close();
				Assert.Fail("It's valid credentials");
			}

			// Error message displayed -> success
			if (driver.FindElement(By.JQuerySelector(".oxd-alert-content.oxd-alert-content--error > p")).Displayed)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "InvalidUser", "Pass", 4);
				driver.Close();
				Assert.Pass("Test Login with invalid user success !");
			}
		}

		[Test, Category("Login")]
		//[TestCase(TestName = "Logout redirects to login")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetValidUserDatasFromExcel")]
		public void Logout_FromHomePage_RedirectToLogin(string username, string password)
		{
			Login_WithValidUser_NavigatesToDashboardPage(username, password);

			// Wait and Click user info
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-topbar-header-userarea > ul > li")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-topbar-header-userarea > ul > li")).Click();

			// Wait and Click logout
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-topbar-header-userarea > ul > li > ul > li:nth-child(4)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-topbar-header-userarea > ul > li > ul > li:nth-child(4)")).Click();

			// Wait login button in Login page and check visible
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-form-actions.orangehrm-login-action > button")).Count > 0);
			}

			Thread.Sleep(1000);

			if (driver.FindElement(By.JQuerySelector(".oxd-form-actions.orangehrm-login-action > button")).Displayed)
			{
				driver.Close();
				Assert.Pass("Test Logout user success !");
			}
		}
	}
}
