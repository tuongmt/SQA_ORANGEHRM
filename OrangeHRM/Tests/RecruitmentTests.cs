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

namespace OrangeHRM.Tests
{
	internal class RecruitmentTests
	{
		private IWebDriver _driver;
		private IJavaScriptExecutor _js;

		public RecruitmentTests() {}

		[SetUp]
		public void Setup()
		{
			ChromeOptions options = new ChromeOptions();
			options.AddArgument("--start-maximized");
			ChromeDriverService service = ChromeDriverService.CreateDefaultService("");
			_driver = new ChromeDriver(service, options);
		}

		[TearDown]
		public void TearDown()
		{
			_driver.Quit();
		}

		[Test, Category("Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetAddVacancyDatasFromExcel")]
		public void ExecRecruitment_AddVacancy(string username, string password, string vacancyName, string jobTitle, string hiringManager)
		{
			LoginPage loginPage = new LoginPage(_driver, _js);
			RecruitmentPage recruitmentPage = new RecruitmentPage(_driver, _js);

			loginPage.GetAPI();
			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			recruitmentPage.FlowEnteringVacancies();
			recruitmentPage.Recruitment_AddVacancy(vacancyName, jobTitle, hiringManager);
		}

		[Test, Category("Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetDeleteVacancyDatasFromExcel")]
		public void ExecRecruitment_DeleteVacancy(string username, string password, string vacancyNo)
		{
			LoginPage loginPage = new LoginPage(_driver, _js);
			RecruitmentPage recruitmentPage = new RecruitmentPage(_driver, _js);

			loginPage.GetAPI();

			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			recruitmentPage.FlowEnteringVacancies();
			recruitmentPage.Recruitment_DeleteVacancy(vacancyNo);
		}

		[Test, Category("Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetAddCandidateDatasFromExcel")]
		public void ExecRecruitment_AddCandidate(string username, string password, string firstName, string middleName, string lastName, string vacancy, string email)
		{
			LoginPage loginPage = new LoginPage(_driver, _js);
			RecruitmentPage recruitmentPage = new RecruitmentPage(_driver, _js);

			loginPage.GetAPI();

			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			recruitmentPage.FlowEnteringCandidates();
			recruitmentPage.Recruitment_AddCandidate(firstName, middleName, lastName, vacancy, email);
		}

		[Test, Category("Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetDeleteCandidateDatasFromExcel")]
		public void ExecRecruitment_DeleteCandidate(string username, string password, string candidateNo)
		{
			LoginPage loginPage = new LoginPage(_driver, _js);
			RecruitmentPage recruitmentPage = new RecruitmentPage(_driver, _js);

			loginPage.GetAPI();

			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			recruitmentPage.FlowEnteringCandidates();
			recruitmentPage.Recruitment_DeleteCandidate(candidateNo);
		}

		// Unsuccessful
		[Test, Category("Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetEditVacancyDatasFromExcel")]
		public void ExecRecruitment_EditVacancy(string username, string password, string vacancyNo, string vacancyName, string jobTitle, string hiringManager)
		{
			LoginPage loginPage = new LoginPage(_driver, _js);
			RecruitmentPage recruitmentPage = new RecruitmentPage(_driver, _js);

			loginPage.GetAPI();

			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			recruitmentPage.FlowEnteringVacancies();
			recruitmentPage.Recruitment_EditVacancy(vacancyNo, vacancyName, jobTitle, hiringManager);
		}

		[Test, Category("Recruitment")]
		[TestCaseSource(typeof(ExcelDataProvider), "GetViewCandidateDatasFromExcel")]
		public void Recruitment_ViewCandidate(string username, string password, string candidateNo)
		{

			LoginPage loginPage = new LoginPage(_driver, _js);
			RecruitmentPage recruitmentPage = new RecruitmentPage(_driver, _js);

			loginPage.GetAPI();

			loginPage.Login_WithValidUser_NavigatesToDashboardPage(username, password);

			recruitmentPage.FlowEnteringCandidates();
			recruitmentPage.Recruitment_ViewCandidate(candidateNo);
		}
	}
}
