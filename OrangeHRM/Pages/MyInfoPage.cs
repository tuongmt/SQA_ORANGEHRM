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

namespace OrangeHRM.Pages
{
	internal class MyInfoPage
	{
		private IWebDriver _driver;
		private IJavaScriptExecutor _js;

		public MyInfoPage(IWebDriver driver, IJavaScriptExecutor js)
		{
			this._driver = driver;
			this._js = js;
		}

		public void FlowEnteringMyInfo()
		{
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(_driver => _driver.FindElements(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(6)")).Count > 0);
			}
			_driver.FindElement(By.CssSelector(".oxd-main-menu-item-wrapper:nth-child(6)")).Click();
		}

		public void MyInfo_ChangeProfilePicture(string path)
		{
			// Wait and Click change picture
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".orangehrm-edit-employee-imagesection > div.orangehrm-edit-employee-image-wrapper > div > img")).Count > 0);
			}
			Thread.Sleep(2000);
			_driver.FindElement(By.CssSelector(".orangehrm-edit-employee-imagesection > div.orangehrm-edit-employee-image-wrapper > div > img")).Click();

			// Wait and Fill Path
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector(".oxd-form-row > div > div > div:nth-child(2) > div > button")).Count > 0);
			}

			_driver.FindElement(By.CssSelector("form > div.oxd-form-row > div > div > div:nth-child(2) > input")).SendKeys(path);
			Thread.Sleep(2000);

			// Click save
			_driver.FindElement(By.CssSelector(".orangehrm-edit-employee-content > div > form > div.oxd-form-actions > button")).Click();

			ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "ChangeProfilePicture", "Pass", 5);
			_driver.Close();
			Assert.Pass("Change Profile Picture success");
		}

		public void MyInfo_EditPersonalDetails(string firstName, string middleName, string lastName, 
			string employeeId, string otherId, string licenseNumber, string licenseExpiryDate,
			string nationality, string maritalStatus, string dob, string gender)
		{
			// Wait and Send firstName
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector("form > div:nth-child(1) > div > div > div > div.--name-grouped-field > div:nth-child(1) > div:nth-child(2) > input")).Count > 0);
			}
			var fillFirstName = _driver.FindElement(By.CssSelector("form > div:nth-child(1) > div > div > div > div.--name-grouped-field > div:nth-child(1) > div:nth-child(2) > input"));
			fillFirstName.SendKeys(Keys.Control + "a");
			fillFirstName.SendKeys(Keys.Delete);
			fillFirstName.SendKeys(firstName);
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => driver.FindElements(By.CssSelector("form > div:nth-child(1) > div > div > div > div.--name-grouped-field > div:nth-child(1) > span")).Count == 0);
				}
			}catch(TimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "EditPersonalDetails", "FirstName is required", 15);
				_driver.Close();
				Assert.Fail("FirstName is required");
			}

			//Send middleName
			var fillMiddleName = _driver.FindElement(By.CssSelector("form > div:nth-child(1) > div > div > div > div.--name-grouped-field > div:nth-child(2) > div:nth-child(2) > input"));
			fillMiddleName.SendKeys(Keys.Control + "a");
			fillMiddleName.SendKeys(Keys.Delete);
			fillMiddleName.SendKeys(middleName);

			//Send lastName
			var fillLastName = _driver.FindElement(By.CssSelector("form > div:nth-child(1) > div > div > div > div.--name-grouped-field > div:nth-child(3) > div:nth-child(2) > input"));
			fillLastName.SendKeys(Keys.Control + "a");
			fillLastName.SendKeys(Keys.Delete);
			fillLastName.SendKeys(lastName);
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => driver.FindElements(By.CssSelector("form > div:nth-child(1) > div > div > div > div.--name-grouped-field > div:nth-child(3) > span")).Count == 0);
				}
			}
			catch (TimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "EditPersonalDetails", "LastName is required", 15);
				_driver.Close();
				Assert.Fail("LastName is required");
			}

			//Send employeeId
			var fillEmployeeId = _driver.FindElement(By.CssSelector("form > div:nth-child(3) > div:nth-child(1) > div:nth-child(1) > div > div:nth-child(2) > input"));
			fillEmployeeId.SendKeys(Keys.Control + "a");
			fillEmployeeId.SendKeys(Keys.Delete);
			fillEmployeeId.SendKeys(employeeId);

			//Send otherId
			var fillOtherId = _driver.FindElement(By.CssSelector("form > div:nth-child(3) > div:nth-child(1) > div:nth-child(2) > div > div:nth-child(2) > input"));
			fillOtherId.SendKeys(Keys.Control + "a");
			fillOtherId.SendKeys(Keys.Delete);
			fillOtherId.SendKeys(otherId);

			//Send licenseNumber
			var fillLicenseNumber = _driver.FindElement(By.CssSelector("form > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div > div:nth-child(2) > input"));
			fillLicenseNumber.SendKeys(Keys.Control + "a");
			fillLicenseNumber.SendKeys(Keys.Delete);
			fillLicenseNumber.SendKeys(licenseNumber);

			//Send licenseExpiryDate
			var fillLicenseExpiryDate = _driver.FindElement(By.CssSelector("form > div:nth-child(3) > div:nth-child(2) > div:nth-child(2) > div > div:nth-child(2) > div > div > input"));
			fillLicenseExpiryDate.SendKeys(Keys.Control + "a");
			fillLicenseExpiryDate.SendKeys(Keys.Delete);
			fillLicenseExpiryDate.SendKeys(licenseExpiryDate);
			try
			{
				{
					WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
					wait.Until(driver => driver.FindElements(By.CssSelector("form > div:nth-child(3) > div:nth-child(2) > div:nth-child(2) > div > span")).Count == 0);
				}
			}
			catch (TimeoutException)
			{
				ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "EditPersonalDetails", "Should be a valid date in yyyy-mm-dd format", 15);
				_driver.Close();
				Assert.Fail("Should be a valid date in yyyy-mm-dd format");
			}

			//Send nationality
			_driver.FindElement(By.CssSelector("form > div:nth-child(5) > div:nth-child(1) > div:nth-child(1) > div > div:nth-child(2) > div > div > div.oxd-select-text-input")).Click();
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector("form > div:nth-child(5) > div:nth-child(1) > div:nth-child(1) > div > div:nth-child(2) > div > div.oxd-select-dropdown.--positon-bottom > .oxd-select-option > span")).Count > 0);
			}
			_driver.FindElement(By.JQuerySelector($"form > div:nth-child(5) > div:nth-child(1) > div:nth-child(1) > div > div:nth-child(2) > div > div.oxd-select-dropdown.--positon-bottom > .oxd-select-option > span:contains('{nationality}')")).Click();

			//Send Marital Status
			_driver.FindElement(By.CssSelector("form > div:nth-child(5) > div:nth-child(1) > div:nth-child(2) > div > div:nth-child(2) > div > div > div.oxd-select-text-input")).Click();
			{
				WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(10));
				wait.Until(driver => driver.FindElements(By.CssSelector("form > div:nth-child(5) > div:nth-child(1) > div:nth-child(2) > div > div:nth-child(2) > div > div.oxd-select-dropdown.--positon-bottom > .oxd-select-option > span")).Count > 0);
			}
			_driver.FindElement(By.JQuerySelector($"form > div:nth-child(5) > div:nth-child(1) > div:nth-child(2) > div > div:nth-child(2) > div > div.oxd-select-dropdown.--positon-bottom > .oxd-select-option > span:contains('{maritalStatus}')")).Click();

			//Send DOB
			var fillDOB = _driver.FindElement(By.CssSelector("form > div:nth-child(5) > div:nth-child(2) > div:nth-child(1) > div > div:nth-child(2) > div > div > input"));
			fillDOB.SendKeys(Keys.Control + "a");
			fillDOB.SendKeys(Keys.Delete);
			fillDOB.SendKeys(dob);

			//Click Gender (1 or 2)
			 _driver.FindElement(By.CssSelector($".--gender-grouped-field > div:nth-child({gender}) > div:nth-child(2) > div > label > span")).Click();
			Thread.Sleep(1);
			// Click save
			_driver.FindElement(By.CssSelector("form > div.oxd-form-actions > button")).Click();

			ExcelDataProvider.WriteResultToExcel("TestCaseData_Tuong.xlsx", "EditPersonalDetails", "Pass", 15);
			_driver.Close();
			Assert.Pass("Edit Personal Details success");
		}
	}
}
