using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using By = Selenium.WebDriver.Extensions.By;

namespace OrangeHRM.Tests
{
	internal class RecruitmentTests
	{
		private IWebDriver driver;
		private LoginTests loginTests = new LoginTests();

		public void Setup()
		{
			loginTests.Setup();
			driver = loginTests.driver;
		}
		
		[Test, Category("Recruitment")]
		//[TestCase(TestName = "Add Vancancy from Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetAddVacancyDatasFromExcel")]
		public void Recruitment_AddVacancy(string username, string password, string vacancyName, string jobTitle, string hiringManager)
		{
			Setup();
			
			// Login
			loginTests.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			// Click recruitment in dashboard list
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Click();

			// choose vancancies
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Click();

			// choose add
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-button--secondary:nth-child(1)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-button--secondary:nth-child(1)")).Click();

			// fill vancancy name
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--active")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--active")).Click();
			//driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--focus")).SendKeys("Tester1");	
			driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--focus")).SendKeys(vacancyName);

			// choose dropdown
			driver.FindElement(By.JQuerySelector(".oxd-select-text-input")).Click();
			// find option
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-select-option > span")).Count > 0);
			}
			//driver.FindElement(By.JQuerySelector(".oxd-select-option > span:contains('Web developer')")).Click();

			driver.FindElement(By.JQuerySelector($".oxd-select-option > span:contains('{jobTitle}')")).Click();

			// fill hiring manager 
			driver.FindElement(By.JQuerySelector(".oxd-autocomplete-text-input--active > input")).SendKeys(hiringManager);
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.JQuerySelector($".oxd-autocomplete-option > span:contains('{hiringManager}')")).Count > 0);
			}
			driver.FindElement(By.JQuerySelector($".oxd-autocomplete-option > span:contains('{hiringManager}')")).Click();

			driver.FindElement(By.CssSelector("button.oxd-button.oxd-button--medium.oxd-button--secondary.orangehrm-left-space")).Click();
			
			Thread.Sleep(2000);
			TestContext.Out.WriteLine("Add vancancies success");

			driver.Close();
		}


	}
}
