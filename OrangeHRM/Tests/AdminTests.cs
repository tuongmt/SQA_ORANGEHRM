using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using By = Selenium.WebDriver.Extensions.By;
using System.Threading;
using System.Security.Cryptography;

namespace OrangeHRM.Tests
{
	internal class AdminTests
	{
		private IWebDriver driver;
		private LoginTests loginTests = new LoginTests();

		public void Setup()
		{
			loginTests.Setup();
			driver = loginTests.driver;
		}

		[Test, Category("Admin")]
		//[TestCase(TestName = "Add JobTitle from Admin")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetAddJobTitleDatasFromExcel")]
		public void Admin_AddJobTitle(string username, string password, string jobTitle)
		{
			Setup();

			// Login
			loginTests.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			// Click admin in dashboard list, check it visible???
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(1)")).Count > 0);
			}//:nth-child(1) la phan tu thu nhat trong tap hop .oxd-main-menu-item-wrapper
			driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(1)")).Click();

			// choose job from navbar, tuong tu phia tren
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Count > 0);
			}//:nth-child(2) chon cai thu hai
			driver.FindElement(By.CssSelector(".oxd-topbar-body-nav-tab:nth-child(2)")).Click();

			// choose job title from job list
			{
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".--active.oxd-topbar-body-nav-tab.--parent > ul > li:nth-child(1)")).Count > 0);
			}//.--active.oxd-topbar-body-nav-tab.--parent.--visited > ul > li:nth-child(1) > a // rut gon do dai
			driver.FindElement(By.CssSelector(".--active.oxd-topbar-body-nav-tab.--parent > ul > li:nth-child(1)")).Click();


			// choose add
			{// thu bo :nth-child(1) xem
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-button--secondary")).Count > 0);
			}
			driver.FindElement(By.CssSelector(".oxd-button--secondary")).Click();

			// fill Job Title name
			{ //form > div:nth-child(1) > div > div:nth-child(2) > input rut gon tu form de lay phan tu con
				WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));
				wait.Until(driver => driver.FindElements(By.CssSelector("form > div:nth-child(1) > div > div:nth-child(2) > input")).Count > 0);
			}
			driver.FindElement(By.CssSelector("form > div:nth-child(1) > div > div:nth-child(2) > input")).Click();
			driver.FindElement(By.CssSelector("form > div:nth-child(1) > div > div:nth-child(2) > input")).SendKeys(jobTitle);

			// click save //button.oxd-button.oxd-button--medium.oxd-button--secondary.orangehrm-left-space rut gon bot
			driver.FindElement(By.CssSelector("button.oxd-button.oxd-button--medium.oxd-button--secondary.orangehrm-left-space")).Click();

			Thread.Sleep(2000);
			TestContext.Out.WriteLine("Add job title success");

			driver.Close();
		}
	}
}
