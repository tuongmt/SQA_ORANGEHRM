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
		
		public void FlowEnteringCandidates()
		{
			// Click Recruitment in dashboard list
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Click();
		}

		public void FlowEnteringVacancies()
		{
			// Click Recruitment in dashboard list
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Click();

			// Choose Vacancies in navbar Recruitment
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Click();
		}

		[Test, Category("Recruitment")]
		//[TestCase(TestName = "Add Vancancy from Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetAddVacancyDatasFromExcel")]
		public void Recruitment_AddVacancy(string username, string password, string vacancyName, string jobTitle, string hiringManager)
		{
			Setup();
			
			// Login
			loginTests.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			FlowEnteringVacancies();
			
			// Choose add button
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Click();

			// Fill Vacancy Name
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--active")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--active")).Click();
			driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--focus")).SendKeys(vacancyName);

			// Wait Error message when Vacancy Name is exist
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => driver.FindElements(By.CssSelector("form > div:nth-child(1) > div:nth-child(1) > div > span")).Count == 0);
				}
			}
			catch (WebDriverTimeoutException){
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "AddVacancy", "Fail (Vacancy Name is exist)", 7);
				driver.Close();
				Assert.Fail("Vacancy Name is exist !");
			}

			// Wait and choose option Job Title
			driver.FindElement(By.JQuerySelector(".oxd-select-text-input")).Click();
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-select-option > span")).Count > 0);
			}
			try {
				driver.FindElement(By.JQuerySelector($".oxd-select-option > span:contains('{jobTitle}')")).Click();
			}
			catch (NoSuchElementException){
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "AddVacancy", "Fail (Job Title isn't exist)", 7);
				driver.Close();
				Assert.Fail("Job Title isn't exist !");
			}

			// Fill and Choose option Hiring Manager
			driver.FindElement(By.JQuerySelector(".oxd-autocomplete-text-input--active > input")).SendKeys(hiringManager);
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => driver.FindElements(By.JQuerySelector($".oxd-autocomplete-option > span:contains('{hiringManager}')")).Count > 0);
				}
				driver.FindElement(By.JQuerySelector($".oxd-autocomplete-option > span:contains('{hiringManager}')")).Click();
			}
			catch (WebDriverTimeoutException){
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "AddVacancy", "Fail (Hiring Manager isn't exist)", 7);
				driver.Close();
				Assert.Fail("hiringManager isn't exist !");
			}

			// Click Save button
			driver.FindElement(By.CssSelector("button.oxd-button--secondary.orangehrm-left-space")).Click();

			ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "AddVacancy", "Pass", 7);
			driver.Close();
			Assert.Pass("Add vacancy success !");
			
		}

		[Test, Category("Recruitment")]
		//[TestCase(TestName = "Delete Vancancy from Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetDeleteVacancyDatasFromExcel")]
		public void Recruitment_DeleteVacancy(string username, string password, string vacancyNo)
		{
			Setup();

			// Login
			loginTests.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			FlowEnteringVacancies();

			// Click delete button from vacancy card and check vacancyNo (item in this) exist or not.
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector($"div.card-item.card-header-slot-content.--right > div > div > button:nth-child(1)")).Count > 0);
			}
			try
			{
				driver.FindElement(By.CssSelector($"div:nth-child({vacancyNo}) > div > div > div.card-header-slot > div.card-item.card-header-slot-content.--right > div > div > button:nth-child(1)")).Click();

				// Click Yes, delete oxd-button oxd-button--medium oxd-button--text orangehrm-button-margin
				{
					WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--label-danger.orangehrm-button-margin")).Count > 0);
				}
				driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--label-danger.orangehrm-button-margin")).Click();

				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "DeleteVacancy", "Pass", 5);
				driver.Close();
				Assert.Pass("Delete vacancy success");
			}
			catch (NoSuchElementException){
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "DeleteVacancy", "Fail (Vacancy No not found)", 5);
				driver.Close();
				Assert.Fail("vacancyNo not found. Delete vacancy fail !");
			}
		}

		[Test, Category("Recruitment")]
		//[TestCase(TestName = "Add Candidate from Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetAddCandidateDatasFromExcel")]
		public void Recruitment_AddCandidate(string username, string password, string firstName, string middleName, string lastName, string vacancy, string email)
		{
			Setup();

			// Login
			loginTests.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			FlowEnteringCandidates();

			// Click Add button
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Click();

			// Fill first name
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.Name("firstName")).Count > 0);
			}
			driver.FindElement(By.Name("firstName")).SendKeys(firstName);

			// Fill middle name
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.Name("middleName")).Count > 0);
			}
			driver.FindElement(By.Name("middleName")).SendKeys(middleName);

			// Fill last name
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.Name("lastName")).Count > 0);
			}
			driver.FindElement(By.Name("lastName")).SendKeys(lastName);

			// Choose option Vacancy
			driver.FindElement(By.JQuerySelector(".oxd-select-text-input")).Click();
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-select-dropdown > .oxd-select-option > span")).Count > 0);
			}
			try{
				driver.FindElement(By.JQuerySelector($".oxd-select-dropdown > .oxd-select-option > span:contains('{vacancy}')")).Click();
			}
			catch (NoSuchElementException) { 
				driver.Close();
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "AddCandidate", "Fail (Vacancy not found)", 9);
				Assert.Fail("Vacancy not found!"); }

			// Fill email  
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector("div:nth-child(1) > div > div:nth-child(2) > input")).Count > 0);
			}
			driver.FindElement(By.CssSelector("div:nth-child(1) > div > div:nth-child(2) > input")).SendKeys(email);

			try {
				{
					WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => driver.FindElements(By.CssSelector("div > form > div:nth-child(3) > div > div:nth-child(1) > div > span")).Count == 0);
				}
			}
			catch (WebDriverTimeoutException){ 
				driver.Close();
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "AddCandidate", "Fail (Email is invalid format)", 9);
				Assert.Fail("Email is invalid format !"); }

			Thread.Sleep(2000); // use to debug, dont save result

			driver.FindElement(By.CssSelector("button.oxd-button--secondary.orangehrm-left-space")).Click();

			ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "AddCandidate", "Pass", 9);
			driver.Close();
			Assert.Pass("Add Candidate success !");
		}

		[Test, Category("Recruitment")]
		//[TestCase(TestName = "Delete Candidate from Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetDeleteCandidateDatasFromExcel")]
		public void Recruitment_DeleteCandidate(string username, string password, string candidateNo)
		{
			Setup();

			// Login
			loginTests.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			FlowEnteringCandidates();

			//Wait candidates info and Click delete item(candidateNo) 
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".card-item.card-header-slot-content.--right > div > div > button:nth-child(2)")).Count > 0);
			}
			try
			{
				driver.FindElement(By.CssSelector($"div:nth-child({candidateNo}) > div > div > .card-header-slot > .card-item.card-header-slot-content.--right > div > div > button:nth-child(2)")).Click();
				Thread.Sleep(1);
			}
			catch (NoSuchElementException){
				driver.Close();
				ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "DeleteCandidate", "Fail (Candidate No not found)", 5);
				Assert.Fail("Candidate No not found !");
			}

			//Wait popup showing and click Yes, delete
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--label-danger.orangehrm-button-margin")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--label-danger.orangehrm-button-margin")).Click();

			driver.Close();
			ExcelDataProvider.WriteResultToExcel("TestCaseData.xlsx", "DeleteCandidate", "Pass", 5);
			Assert.Pass("Delete Candidate success !");
		}	
	}
}
