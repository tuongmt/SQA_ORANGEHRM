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

namespace OrangeHRM.Pages
{
	internal class LoginPage
	{
		public IWebDriver _driver;
		private IJavaScriptExecutor _js;

		public LoginPage(IWebDriver driver, IJavaScriptExecutor js)
		{
			this._driver = driver;
			this._js = js;
		}

		[Test]
		public void GetAPI()
		{
			string URL = "http://localhost/orangehrm-5.4/orangehrm-5.4/web/index.php/auth/login";
			_driver.Navigate().GoToUrl(URL);
			_driver.Manage().Window.Position = new System.Drawing.Point(0, 0);
			_driver.Manage().Window.Size = new System.Drawing.Size(1000, 825);
			Thread.Sleep(3000);
			//driver.Close();
		}

		[Test, Category("Login")]
		public void Login_WithValidUser_NavigatesToDashboardPage(string username, string password)
		{
			_driver.FindElement(By.Name("username")).SendKeys(username);
			_driver.FindElement(By.Name("password")).SendKeys(password);
			_driver.FindElement(By.CssSelector("button[type='submit']")).Click();

			// Check input required username when login
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => _driver.FindElements(By.CssSelector("form > div:nth-child(2) > div > span")).Count == 0);
				}
			}
			catch (WebDriverTimeoutException)
			{
				var usernameErrorMessage = _driver.FindElement(By.JQuerySelector("form > div:nth-child(2) > div > span"));
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ValidUser", usernameErrorMessage.Text, 4);
				Assert.Fail("Username " + usernameErrorMessage.Text);
				_driver.Close();
			}

			// Check input required password when login
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => _driver.FindElements(By.CssSelector("form > div:nth-child(3) > div > span")).Count == 0);
				}
			}
			catch (WebDriverTimeoutException)
			{
				var passwordErrorMessage = _driver.FindElement(By.JQuerySelector("form > div:nth-child(3) > div > span"));
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ValidUser", passwordErrorMessage.Text, 4);
				Assert.Fail("Password " + passwordErrorMessage.Text);
				_driver.Close();
			}


			// Check error message when login fail
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => _driver.FindElements(By.CssSelector(".oxd-alert-content.oxd-alert-content--error > p")).Count == 0);
			}
			try
			{
				if (_driver.FindElement(By.JQuerySelector(".oxd-alert-content.oxd-alert-content--error > p")).Displayed)
				{
					var invalidCredentials = _driver.FindElement(By.JQuerySelector(".oxd-alert-content.oxd-alert-content--error > p"));
					ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ValidUser", invalidCredentials.Text, 4);
					Assert.Fail(invalidCredentials.Text);
					_driver.Close();
				}
			}
			catch (NoSuchElementException)
			{
				
			}

			// If login success redirects to homepage and see dashboard
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => _driver.FindElements(By.CssSelector(".oxd-sidepanel-body > ul > li:nth-child(8) > a > span")).Count > 0);
				}
			}
			catch (WebDriverTimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ValidUser", "Not redirect to Dashboard page", 4);
				Assert.Fail("Not redirect to Dashboard page");
				_driver.Close();
			}

			// Only using when running this function
			//var dashBoardPage = _driver.FindElement(By.JQuerySelector(".oxd-sidepanel-body > ul > li:nth-child(8) > a > span"));
			//string textContent = dashBoardPage.Text;
			//ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ValidUser", "Redirect to " + textContent + " page", 4);
			//Assert.That("Redirect to Dashboard page", Is.EqualTo("Redirect to " + textContent + " page"));
			//_driver.Close();
		}

		[Test, Category("Login")]
		public void Login_WithInvalidUser_ShowsErrorMessage(string username, string password)
		{
			_driver.FindElement(By.Name("username")).SendKeys(username);
			_driver.FindElement(By.Name("password")).SendKeys(password);
			_driver.FindElement(By.CssSelector("button[type='submit']")).Click();

			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(username)) {
				// Check input required username when login
				try
				{
					{
						WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
						wait.Until(driver => _driver.FindElements(By.CssSelector("form > div:nth-child(2) > div > span")).Count > 0);
					}
				}
				catch (WebDriverTimeoutException)
				{
					ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "InvalidUser", "It's valid credentials", 4);
					Assert.Fail("It's valid credentials");
					_driver.Close();
				}

				// Error message displayed -> success
				if (_driver.FindElement(By.JQuerySelector("form > div:nth-child(2) > div > span")).Displayed)
				{
					var usernameErrorMessage = _driver.FindElement(By.JQuerySelector("form > div:nth-child(2) > div > span"));
					ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "InvalidUser", usernameErrorMessage.Text, 4);
					Assert.That(usernameErrorMessage.Text, Is.EqualTo("Required"));
					_driver.Close();
				}
			}
			else if(string.IsNullOrWhiteSpace(password) || string.IsNullOrEmpty(password))
			{
				// Check input required password when login
				try
				{
					{
						WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
						wait.Until(driver => _driver.FindElements(By.CssSelector("form > div:nth-child(3) > div > span")).Count > 0);
					}
				}
				catch (WebDriverTimeoutException)
				{
					ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "InvalidUser", "It's valid credentials", 4);
					Assert.Fail("It's valid credentials");
					_driver.Close();
				}

				// Error message displayed -> success
				if (_driver.FindElement(By.JQuerySelector("form > div:nth-child(3) > div > span")).Displayed)
				{
					var passwordErrorMessage = _driver.FindElement(By.JQuerySelector("form > div:nth-child(3) > div > span"));
					ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "InvalidUser", passwordErrorMessage.Text, 4);
					Assert.That(passwordErrorMessage.Text, Is.EqualTo("Required"));
					_driver.Close();
				}
			}
			else
			{
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
					ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "InvalidUser", "It's valid credentials", 4);
					Assert.Fail("It's valid credentials"); _driver.Close();
				}

				// Error message displayed -> success
				if (_driver.FindElement(By.JQuerySelector(".oxd-alert-content.oxd-alert-content--error > p")).Displayed)
				{
					var invalidErrorMessage = _driver.FindElement(By.JQuerySelector(".oxd-alert-content.oxd-alert-content--error > p"));
					ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "InvalidUser", invalidErrorMessage.Text, 4);
					Assert.That(invalidErrorMessage.Text, Is.EqualTo("Invalid credentials"));
					_driver.Close();
				}
			}
		}

		[Test, Category("Login")]
		public void Logout_FromHomePage_RedirectToLogin(string username, string password)
		{
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

			// Wait login page and check visible
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".orangehrm-login-slot > h5")).Count > 0);
			}
			if(_driver.FindElement(By.JQuerySelector(".orangehrm-login-slot > h5")).Displayed)
			{
				var loginPage = _driver.FindElement(By.JQuerySelector(".orangehrm-login-slot > h5"));
				Assert.That(loginPage.Text + " page", Is.EqualTo("Login page"));
				_driver.Close();
			}
			else
			{
				Assert.Fail("Return another page");
				_driver.Close();
			}
		}
	}
}
