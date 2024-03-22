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
using OrangeHRM.Pages;

namespace OrangeHRM.Pages
{
	internal class RecruitmentPage
	{
		private IWebDriver _driver;
		private IJavaScriptExecutor _js;

		public RecruitmentPage(IWebDriver driver, IJavaScriptExecutor js)
		{
			_driver = driver;
			_js = js;
		}

		public void FlowEnteringCandidates()
		{
			// Click Recruitment in dashboard list
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Click();
		}

		public void FlowEnteringVacancies()
		{
			// Click Recruitment in dashboard list
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Click();

			// Choose Vacancies in navbar Recruitment
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Click();
		}

		public void Recruitment_AddVacancy(string vacancyName, string jobTitle, string hiringManager)
		{
			//Setup();

			// Login
			//loginTests.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			//FlowEnteringVacancies();

			// Choose add button
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Click();

			// Fill Vacancy Name
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--active")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--active")).Click();
			_driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--focus")).SendKeys(vacancyName);

			// Wait Error message when Vacancy Name is exist
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(_driver => _driver.FindElements(By.CssSelector("form > div:nth-child(1) > div:nth-child(1) > div > span")).Count == 0);
				}
			}
			catch (WebDriverTimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddVacancy", "Fail (Vacancy Name is exist)", 7);
				_driver.Close();
				Assert.Fail("Vacancy Name is exist !");
			}

			// Wait and choose option Job Title
			_driver.FindElement(By.JQuerySelector(".oxd-select-text-input")).Click();
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-select-option > span")).Count > 0);
			}
			try
			{
				_driver.FindElement(By.JQuerySelector($".oxd-select-option > span:contains('{jobTitle}')")).Click();
			}
			catch (NoSuchElementException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddVacancy", "Fail (Job Title isn't exist)", 7);
				_driver.Close();
				Assert.Fail("Job Title isn't exist !");
			}

			// Fill and Choose option Hiring Manager
			_driver.FindElement(By.JQuerySelector(".oxd-autocomplete-text-input--active > input")).SendKeys(hiringManager);
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(_driver => _driver.FindElements(By.JQuerySelector($".oxd-autocomplete-option > span:contains('{hiringManager}')")).Count > 0);
				}
				_driver.FindElement(By.JQuerySelector($".oxd-autocomplete-option > span:contains('{hiringManager}')")).Click();
			}
			catch (WebDriverTimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddVacancy", "Fail (Hiring Manager isn't exist)", 7);
				_driver.Close();
				Assert.Fail("hiringManager isn't exist !");
			}

			// Click Save button
			_driver.FindElement(By.CssSelector("button.oxd-button--secondary.orangehrm-left-space")).Click();

			ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddVacancy", "Pass", 7);
			_driver.Close();
			Assert.Pass("Add vacancy success !");
		}

		public void Recruitment_DeleteVacancy(string vacancyNo)
		{

			// Click delete button from vacancy card and check vacancyNo (item in this) exist or not.
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector($"div.card-item.card-header-slot-content.--right > div > div > button:nth-child(1)")).Count > 0);
			}
			try
			{
				_driver.FindElement(By.CssSelector($"div:nth-child({vacancyNo}) > div > div > div.card-header-slot > div.card-item.card-header-slot-content.--right > div > div > button:nth-child(1)")).Click();

				// Click Yes, delete oxd-button oxd-button--medium oxd-button--text orangehrm-button-margin
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--label-danger.orangehrm-button-margin")).Count > 0);
				}
				_driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--label-danger.orangehrm-button-margin")).Click();

				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "DeleteVacancy", "Pass", 5);
				_driver.Close();
				Assert.Pass("Delete vacancy success");
			}
			catch (NoSuchElementException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "DeleteVacancy", "Fail (Vacancy No not found)", 5);
				_driver.Close();
				Assert.Fail("vacancyNo not found. Delete vacancy fail !");
			}
		}

		public void Recruitment_AddCandidate(string firstName, string middleName, string lastName, string vacancy, string email)
		{
			// Click Add button
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Click();

			// Fill first name
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.Name("firstName")).Count > 0);
			}
			_driver.FindElement(By.Name("firstName")).SendKeys(firstName);

			// Fill middle name
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.Name("middleName")).Count > 0);
			}
			_driver.FindElement(By.Name("middleName")).SendKeys(middleName);

			// Fill last name
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.Name("lastName")).Count > 0);
			}
			_driver.FindElement(By.Name("lastName")).SendKeys(lastName);

			// Choose option Vacancy
			_driver.FindElement(By.JQuerySelector(".oxd-select-text-input")).Click();
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-select-dropdown > .oxd-select-option > span")).Count > 0);
			}
			try
			{
				_driver.FindElement(By.JQuerySelector($".oxd-select-dropdown > .oxd-select-option > span:contains('{vacancy}')")).Click();
			}
			catch (NoSuchElementException)
			{
				_driver.Close();
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddCandidate", "Fail (Vacancy not found)", 9);
				Assert.Fail("Vacancy not found!");
			}

			// Fill email  
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector("div:nth-child(1) > div > div:nth-child(2) > input")).Count > 0);
			}
			_driver.FindElement(By.CssSelector("div:nth-child(1) > div > div:nth-child(2) > input")).SendKeys(email);

			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(_driver => _driver.FindElements(By.CssSelector("div > form > div:nth-child(3) > div > div:nth-child(1) > div > span")).Count == 0);
				}
			}
			catch (WebDriverTimeoutException)
			{
				_driver.Close();
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddCandidate", "Fail (Email is invalid format)", 9);
				Assert.Fail("Email is invalid format !");
			}

			Thread.Sleep(2000); // use to debug, dont save result

			_driver.FindElement(By.CssSelector("button.oxd-button--secondary.orangehrm-left-space")).Click();

			ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "AddCandidate", "Pass", 9);
			_driver.Close();
			Assert.Pass("Add Candidate success !");
		}

		public void Recruitment_DeleteCandidate(string candidateNo)
		{
			//Wait candidates info and Click delete item(candidateNo) 
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".card-item.card-header-slot-content.--right > div > div > button:nth-child(2)")).Count > 0);
			}
			try
			{
				_driver.FindElement(By.CssSelector($"div:nth-child({candidateNo}) > div > div > .card-header-slot > .card-item.card-header-slot-content.--right > div > div > button:nth-child(2)")).Click();
				Thread.Sleep(1);
			}
			catch (NoSuchElementException)
			{
				_driver.Close();
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "DeleteCandidate", "Fail (Candidate No not found)", 5);
				Assert.Fail("Candidate No not found !");
			}

			//Wait popup showing and click Yes, delete
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--label-danger.orangehrm-button-margin")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--label-danger.orangehrm-button-margin")).Click();

			_driver.Close();
			ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "DeleteCandidate", "Pass", 5);
			Assert.Pass("Delete Candidate success !");
		}

		//Unsuccessful
		public void Recruitment_EditVacancy(string vacancyNo, string vacancyName, string jobTitle, string hiringManager)
		{
			// Wait and Choose edit button with item want edit
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(_driver => _driver.FindElements(By.CssSelector($"div:nth-child({vacancyNo}) > div > div > div.card-header-slot > div.card-item.card-header-slot-content.--right > div > div > button:nth-child(2)")).Count > 0);
				}
				_driver.FindElement(By.CssSelector($"div:nth-child({vacancyNo}) > div > div > div.card-header-slot > div.card-item.card-header-slot-content.--right > div > div > button:nth-child(2)")).Click();
			}
			catch (WebDriverTimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "EditVacancy", "Fail (Vacancy No isn't exist)", 8);
				_driver.Close();
				Assert.Fail("Vacancy No isn't exist !");
			}

			// Wait Vacancy Name
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector("form > div:nth-child(1) > div:nth-child(1) > div > div:nth-child(2) > input")).Count > 0);
			}
			//Delete old Vacancy Name
			var fillVacancyName = _driver.FindElement(By.CssSelector("form > div:nth-child(1) > div:nth-child(1) > div > div:nth-child(2) > input"));
			fillVacancyName.SendKeys(Keys.Control + "a");
			fillVacancyName.SendKeys(Keys.Delete);
			//Fill new Vacancy Name
			fillVacancyName.SendKeys(vacancyName);

			// Wait Error message when Vacancy Name is exist 
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(_driver => _driver.FindElements(By.CssSelector("form > div:nth-child(1) > div:nth-child(1) > div > span")).Count == 0);
				}
			}
			catch (WebDriverTimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "EditVacancy", "Fail (Vacancy Name is exist)", 8);
				_driver.Close();
				Assert.Fail("Vacancy Name is exist !");
			}

			// Wait and choose option Job Title
			_driver.FindElement(By.JQuerySelector(".oxd-select-text-input")).Click();
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-select-option > span")).Count > 0);
			}
			try
			{
				_driver.FindElement(By.JQuerySelector($".oxd-select-option > span:contains('{jobTitle}')")).Click();
			}
			catch (NoSuchElementException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "EditVacancy", "Fail (Job Title isn't exist)", 8);
				_driver.Close();
				Assert.Fail("Job Title isn't exist !");
			}

			//Delete old Hiring Manager
			var fillHiringManager = _driver.FindElement(By.JQuerySelector(".oxd-autocomplete-text-input--active > input"));
			fillHiringManager.SendKeys(Keys.Control + "a");
			fillHiringManager.SendKeys(Keys.Delete);
			// Fill new Hiring Manager
			fillHiringManager.SendKeys(hiringManager);

			Thread.Sleep(1);
			//Choose option Hiring Manager
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(_driver => _driver.FindElements(By.JQuerySelector($"form > div:nth-child(3) > div:nth-child(1) > div > div:nth-child(2) > div > div.oxd-autocomplete-dropdown.--positon-bottom > div > span:contains('{hiringManager}')")).Count > 0);
				}
				_driver.FindElement(By.JQuerySelector($"form > div:nth-child(3) > div:nth-child(1) > div > div:nth-child(2) > div > div.oxd-autocomplete-dropdown.--positon-bottom > div > span:contains('{hiringManager}')")).Click();
			}
			catch (WebDriverTimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "EditVacancy", "Fail (Hiring Manager isn't exist)", 8);
				_driver.Close();
				Assert.Fail("hiringManager isn't exist !");
			}

			_driver.FindElement(By.CssSelector("button.oxd-button--secondary.orangehrm-left-space")).Click();

			Thread.Sleep(1);
			ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "EditVacancy", "Pass", 8);
			_driver.Close();
			Assert.Pass("Edit vacancy success !");
		}

		[Test, Category("Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetViewCandidateDatasFromExcel")]
		public void Recruitment_ViewCandidate(string candidateNo)
		{

			//Wait candidates info and Click view item(candidateNo) 
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".card-item.card-header-slot-content.--right > div > div > button:nth-child(1)")).Count > 0);
			}
			try
			{
				_driver.FindElement(By.CssSelector($"div:nth-child({candidateNo}) > div > div > .card-header-slot > .card-item.card-header-slot-content.--right > div > div > button:nth-child(1)")).Click();
			}
			catch (NoSuchElementException)
			{
				_driver.Close();
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ViewCandidate", "Fail (Candidate No not found)", 5);
				Assert.Fail("Candidate No not found !");
			}

			_driver.Close();
			ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ViewCandidate", "Pass", 5);
			Assert.Pass("View Candidate success !");
		}
	}
}
