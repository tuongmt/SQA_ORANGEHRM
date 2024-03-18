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

			// Wait until .oxd-main-menu-item-wrapper:nth-child(5) visible
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Count > 0);
			}
			// Click recruitment in dashboard list when login success (recruitment la phan tu thu 5 trong tap hop)
			driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(5)")).Click();

			// choose vancancies, neu la class them . truoc no (vancancies la phan tu thu 2)
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Click();

			// choose add .oxd-button--medium oxd-button--secondary (co the rut gon bot, neu chi co 1 phan tu co the bo child) // ko dc bo khoang trong neu cung la class
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-button.oxd-button--medium.oxd-button--secondary:nth-child(1)")).Click();

			// fill vancancy name .oxd-input.oxd-input--active do bi trung voi search ben dashboard nen phai lay tu .oxd-input-group.oxd-input-field-bottom-space
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--active")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--active")).Click();
			//driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--focus")).SendKeys("Tester1");	
			driver.FindElement(By.CssSelector(".oxd-input-field-bottom-space > div > .oxd-input.oxd-input--focus")).SendKeys(vacancyName);

			// choose dropdown (phai debug de lay thong tin cua option), tat ignore list truoc khi debug
			// lam sao phai cho hien len cai div listbox do thi ta phai lay div parent cua no debug
			//Step over cho toi khi hien option
			driver.FindElement(By.JQuerySelector(".oxd-select-text-input")).Click();
			// find option
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-select-option > span")).Count > 0);
			}
			// > : dai dien cho child cua no la span, :contains chua thong tin cua option
			// tat debug: tat DOM breakpoints truoc, sau do F8 de thoat debug
			//driver.FindElement(By.JQuerySelector(".oxd-select-option > span:contains('Web developer')")).Click();
			driver.FindElement(By.JQuerySelector($".oxd-select-option > span:contains('{jobTitle}')")).Click();

			// fill hiring manager (nhung cai input co the bi trung nhau nen phai co nhieu parent phia truoc no cang tot)
			// debug tiep tuc de lay hiringManager //phai copy tu elements moi dung 100%
			// check .oxd-autocomplete-option > span:contains('{hiringManager} hien thi hay chua
			driver.FindElement(By.JQuerySelector(".oxd-autocomplete-text-input--active > input")).SendKeys(hiringManager);
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.JQuerySelector($".oxd-autocomplete-option > span:contains('{hiringManager}')")).Count > 0);
			}
			driver.FindElement(By.JQuerySelector($".oxd-autocomplete-option > span:contains('{hiringManager}')")).Click();
			Thread.Sleep(2000); //ok, huong dan het roi do, test di nha ^^

			//button.oxd-button.oxd-button--medium.oxd-button--secondary.orangehrm-left-space nen rut gon bot
			driver.FindElement(By.CssSelector("button.oxd-button--secondary.orangehrm-left-space")).Click();


			TestContext.Out.WriteLine("Add vacancy success");

			driver.Close();
		}


	}
}
