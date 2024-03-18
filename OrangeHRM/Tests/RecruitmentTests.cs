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

			// Click Recruitment in dashboard list
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Click();

			// Choose Vacancies in navbar Recruitment
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Click();

			// Choose add button
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Click();

			// Fill Vacancy Name
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--active")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--active")).Click();
			driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--focus")).SendKeys(vacancyName);

			// Choose option Job Title
			driver.FindElement(By.JQuerySelector(".oxd-select-text-input")).Click();
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-select-option > span")).Count > 0);
			}
			driver.FindElement(By.JQuerySelector($".oxd-select-option > span:contains('{jobTitle}')")).Click();

			// Fill and Choose option Hiring Manager
			driver.FindElement(By.JQuerySelector(".oxd-autocomplete-text-input--active > input")).SendKeys(hiringManager);
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.JQuerySelector($".oxd-autocomplete-option > span:contains('{hiringManager}')")).Count > 0);
			}
			driver.FindElement(By.JQuerySelector($".oxd-autocomplete-option > span:contains('{hiringManager}')")).Click();
			
			Thread.Sleep(2000); // use to debug, dont save result

			// Click Save button
			driver.FindElement(By.CssSelector("button.oxd-button--secondary.orangehrm-left-space")).Click();

			TestContext.Out.WriteLine("Add vacancy success");
			driver.Close();
		}

		[Test, Category("Recruitment")]
		//[TestCase(TestName = "Add Vancancy from Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetAddCandidateDatasFromExcel")]
		public void Recruitment_AddCandidate(string username, string password, string firstName, string middleName, string lastName, string vacancy, string email)
		{
			Setup();

			// Login
			loginTests.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			// Click Recruitment in dashboard list
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Click();

			// Click Add button
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Click();

			// Fill first name
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.Name("firstName")).Count > 0);
			}
			driver.FindElement(By.Name("firstName")).SendKeys(firstName);

			// Fill middle name
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.Name("middleName")).Count > 0);
			}
			driver.FindElement(By.Name("middleName")).SendKeys(middleName);

			// Fill last name
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.Name("lastName")).Count > 0);
			}
			driver.FindElement(By.Name("lastName")).SendKeys(lastName);

			// Choose option Vacancy
			driver.FindElement(By.JQuerySelector(".oxd-select-text-input")).Click();
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-select-dropdown > .oxd-select-option > span")).Count > 0);
			}
			driver.FindElement(By.JQuerySelector($".oxd-select-dropdown > .oxd-select-option > span:contains('{vacancy}')")).Click();

			// Fill email 
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector("div:nth-child(1) > div > div:nth-child(2) > input")).Count > 0);
			}
			driver.FindElement(By.CssSelector("div:nth-child(1) > div > div:nth-child(2) > input")).SendKeys(email);

			Thread.Sleep(2000); // use to debug, dont save result

			driver.FindElement(By.CssSelector("button.oxd-button--secondary.orangehrm-left-space")).Click();

			TestContext.Out.WriteLine("Add Candidate success");
			driver.Close();
		}
	}
}
