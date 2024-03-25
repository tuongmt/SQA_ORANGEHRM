using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using By = Selenium.WebDriver.Extensions.By;
using OpenQA.Selenium.Chrome;

namespace OrangeHRM.Pages
{
	internal class AdminPage
	{
		private IWebDriver _driver;
		private IJavaScriptExecutor _js;

		public AdminPage(IWebDriver driver, IJavaScriptExecutor js)
		{
			this._driver = driver;
			this._js = js;
		}

		public void FlowEnteringAdmin()
		{
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => _driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(1)")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(1)")).Click();
		}

		public void Admin_AddJobTitle(string jobTitle)
		{
			// Wait and Click job from navbar
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => _driver.FindElements(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Click();

			// Click Job Title from job list
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".--active.oxd-topbar-body-nav-tab.--parent > ul > li:nth-child(1)")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".--active.oxd-topbar-body-nav-tab.--parent > ul > li:nth-child(1)")).Click();

			// Wait and Click Add
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-button--secondary")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-button--secondary")).Click();

			// Wait and Fill Job Title Name
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector("form > div:nth-child(1) > div > div:nth-child(2) > input")).Count > 0);
			}	
			_driver.FindElement(By.CssSelector("form > div:nth-child(1) > div > div:nth-child(2) > input")).SendKeys(jobTitle);

			// Wait and Check error message when Job Title Name is exist
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => _driver.FindElements(By.JQuerySelector("form > div:nth-child(1) > div > span")).Count == 0);
				}
			}
			catch (WebDriverTimeoutException)
			{
				var isExist = _driver.FindElement(By.JQuerySelector("form > div:nth-child(1) > div > span"));
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddJobTitle", "Job Title Name " + isExist.Text, 5);
				Assert.Fail("Job Title Name " + isExist.Text);
				_driver.Close();
			}

			// Click save
			_driver.FindElement(By.CssSelector("button.oxd-button.oxd-button--medium.oxd-button--secondary.orangehrm-left-space")).Click();

			//Check Job Title required
			if(string.IsNullOrWhiteSpace(jobTitle) || string.IsNullOrEmpty(jobTitle))
			{
				try
				{
					{
						WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
						wait.Until(driver => _driver.FindElements(By.JQuerySelector("form > div:nth-child(1) > div > span")).Count == 0);
					}
				}
				catch (WebDriverTimeoutException)
				{
					var isRequired = _driver.FindElement(By.JQuerySelector("form > div:nth-child(1) > div > span")).Text;
					ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddJobTitle", "Job Title Name " + isRequired, 5);
					Assert.Fail("Job Title Name " + isRequired);
					_driver.Close();
				}
			}

			// Return to job title page
			try {
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => _driver.FindElements(By.JQuerySelector(".orangehrm-header-container > h6")).Count > 0);
				}
				var jobTitlePage = _driver.FindElement(By.JQuerySelector(".orangehrm-header-container > h6"));
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddJobTitle", "Return to " + jobTitlePage.Text + " page when adding successfully", 5);
				Assert.That("Job Titles", Is.EqualTo(jobTitlePage.Text));
				_driver.Close();
			}
			catch (WebDriverTimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddJobTitle", "Don't return to job title page", 5);
				Assert.Fail("Don't return to job title page");
				_driver.Close();
			}
		}
	}
}
