using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using By = Selenium.WebDriver.Extensions.By;

namespace OrangeHRM.Pages
{
	internal class LoginPage
	{
		public IWebDriver _driver;
		private IJavaScriptExecutor _js;

		public LoginPage() { }

		public LoginPage(IWebDriver driver, IJavaScriptExecutor js)
		{
			this._driver = driver;
			this._js = js;
		}

		[TearDown]
		public void TearDown()
		{
			_driver.Quit();
		}

		[Test]
		public void GetAPI()
		{
			try
			{
				string URL = "http://localhost/orangehrm-5.4/orangehrm-5.4/web/index.php/auth/login";
				_driver.Navigate().GoToUrl(URL);
				_driver.Manage().Window.Position = new System.Drawing.Point(0, 0);
				_driver.Manage().Window.Size = new System.Drawing.Size(1000, 825);
				Thread.Sleep(3000);
				//driver.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error from GetAPI " + ex.ToString());
			}
		}

		
		public void Login_WithValidUser_NavigatesToDashboardPage(string username, string password)
		{
			_driver.FindElement(By.Name("username")).SendKeys(username);
			_driver.FindElement(By.Name("password")).SendKeys(password);
			_driver.FindElement(By.CssSelector("button[type='submit']")).Click();

			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => _driver.FindElements(By.CssSelector(".oxd-alert-content.oxd-alert-content--error > p")).Count == 0);
			}
			try
			{
				// Check error message when login fail
				if (_driver.FindElement(By.JQuerySelector(".oxd-alert-content.oxd-alert-content--error > p")).Displayed)
				{
					ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ValidUser", "Fail (Invalid credentials)", 4);
					_driver.Close();
					Assert.Fail("Username or password is wrong, check again");
				}
			}
			catch (NoSuchElementException) { }

			// If login success redirects to homepage and see userdropdown-tab then login success
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => _driver.FindElements(By.CssSelector(".oxd-sidepanel-body > ul > li:nth-child(8)")).Count > 0);
			}

			// Check visible in Dashboard Page
			if (_driver.FindElements(By.CssSelector(".oxd-sidepanel-body > ul > li:nth-child(8)")).Count == 0)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ValidUser", "Fail (Not redirect to dashboard page)", 4);
				_driver.Close();
				Assert.Pass("Test Login with valid user fail because not redirect to dashboard page.");

			}

			//When not run other function then turn on it
			//ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ValidUser", "Pass", 4);
			//driver.Close();
			//Assert.Pass("Test Login with valid user success");
		}

		[Test, Category("Login")]
		public void Login_WithInvalidUser_ShowsErrorMessage(string username, string password)
		{
			//GetAPI();

			_driver.FindElement(By.Name("username")).SendKeys(username);
			_driver.FindElement(By.Name("password")).SendKeys(password);
			_driver.FindElement(By.CssSelector("button[type='submit']")).Click();
			Thread.Sleep(3000);

			// Wait Error message and check invalid credentials
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-alert-content.oxd-alert-content--error > p")).Count > 0);
				}
			}
			catch (WebDriverTimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "InvalidUser", "Fail (It's valid credentials)", 4);
				_driver.Close();
				Assert.Fail("It's valid credentials");
			}
			catch (NoSuchElementException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "InvalidUser", "Fail (It's valid credentials)", 4);
				_driver.Close();
				Assert.Fail("It's valid credentials");
			}

			// Error message displayed -> success
			if (_driver.FindElement(By.JQuerySelector(".oxd-alert-content.oxd-alert-content--error > p")).Displayed)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "InvalidUser", "Pass", 4);
				_driver.Close();
				Assert.Pass("Test Login with invalid user success !");
			}
		}

		[Test, Category("Login")]
		public void Logout_FromHomePage_RedirectToLogin(string username, string password)
		{
			//Login_WithValidUser_NavigatesToDashboardPage(username, password);

			// Wait and Click user info
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-topbar-header-userarea > ul > li")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-topbar-header-userarea > ul > li")).Click();

			// Wait and Click logout
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-topbar-header-userarea > ul > li > ul > li:nth-child(4)")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-topbar-header-userarea > ul > li > ul > li:nth-child(4)")).Click();

			// Wait login button in Login page and check visible
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-form-actions.orangehrm-login-action > button")).Count > 0);
			}

			Thread.Sleep(1000);

			if (_driver.FindElement(By.JQuerySelector(".oxd-form-actions.orangehrm-login-action > button")).Displayed)
			{
				_driver.Close();
				Assert.Pass("Test Logout user success !");
			}
		}
	}
}
