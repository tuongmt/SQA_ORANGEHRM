using ExcelDataReader;
using NUnit.Framework;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeHRM
{
	internal class ExcelDataProvider
	{
		private static int rowStart = 2;

		public static void WriteResultToExcel(string filePath, string sheetName, string result, int colIndexStart)
		{
			try
			{
				using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filePath)))
				{
					ExcelWorksheet ws = package.Workbook.Worksheets[sheetName] ?? package.Workbook.Worksheets.Add(sheetName);

					ws.Cells[rowStart, colIndexStart].Value = result;

					package.Save();
					rowStart+=1;
				}
			}
			catch(Exception ex) 
			{
				Console.WriteLine($"Loi khi ghi tap tin vao excel " + ex.Message);	
			}
		}

		public static IEnumerable<TestCaseData> GetValidUserDatasFromExcel()
		{
			var testData = new List<TestCaseData>();
			using (var stream = File.Open("TestCaseData.xlsx", FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					var result = reader.AsDataSet();
					var table = result.Tables[0];
					for (int i = 1; i < table.Rows.Count; i++)
					{
						string username = table.Rows[i][0].ToString();
						string password = table.Rows[i][1].ToString();
						testData.Add(new TestCaseData(username, password));
					}
				}
			}
			return testData;
		}

		public static IEnumerable<TestCaseData> GetInvalidUserDatasFromExcel()
		{
			var testData = new List<TestCaseData>();
			using (var stream = File.Open("TestCaseData.xlsx", FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					var result = reader.AsDataSet();
					var table = result.Tables[1];
					for (int i = 1; i < table.Rows.Count; i++)
					{
						string username = table.Rows[i][0].ToString();
						string password = table.Rows[i][1].ToString();
						testData.Add(new TestCaseData(username, password));
					}
				}
			}
			return testData;
		}

		public static IEnumerable<TestCaseData> GetAddJobTitleDatasFromExcel()
		{
			var testData = new List<TestCaseData>();
			using (var stream = File.Open("TestCaseData.xlsx", FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					var result = reader.AsDataSet();
					var table = result.Tables[2]; //tuong duong voi worksheet //start by 0
					for (int i = 1; i < table.Rows.Count; i++)
					{
						string username = table.Rows[i][0].ToString();
						string password = table.Rows[i][1].ToString();
						string jobTitle = table.Rows[i][2].ToString();
						testData.Add(new TestCaseData(username, password, jobTitle));
					}
				}
			}
			return testData;
		}

		public static IEnumerable<TestCaseData> GetAddVacancyDatasFromExcel()
		{
			var testData = new List<TestCaseData>();
			using (var stream = File.Open("TestCaseData.xlsx", FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					var result = reader.AsDataSet();
					var table = result.Tables[3];
					for (int i = 1; i < table.Rows.Count; i++)
					{
						string username = table.Rows[i][0].ToString();
						string password = table.Rows[i][1].ToString();
						string vacancyName = table.Rows[i][2].ToString();
						string jobTitle = table.Rows[i][3].ToString();
						string hiringManager = table.Rows[i][4].ToString();
						testData.Add(new TestCaseData(username, password, vacancyName, jobTitle, hiringManager));
					}
				}
			}
			return testData;
		}

		public static IEnumerable<TestCaseData> GetDeleteVacancyDatasFromExcel()
		{
			var testData = new List<TestCaseData>();
			using (var stream = File.Open("TestCaseData.xlsx", FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					var result = reader.AsDataSet();
					var table = result.Tables[4];
					for (int i = 1; i < table.Rows.Count; i++)
					{
						string username = table.Rows[i][0].ToString();
						string password = table.Rows[i][1].ToString();
						string vacancyNo = table.Rows[i][2].ToString();
						testData.Add(new TestCaseData(username, password, vacancyNo));
					}
				}
			}
			return testData;
		}

		public static IEnumerable<TestCaseData> GetAddCandidateDatasFromExcel()
		{
			var testData = new List<TestCaseData>();
			using (var stream = File.Open("TestCaseData.xlsx", FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					var result = reader.AsDataSet();
					var table = result.Tables[5];
					for (int i = 1; i < table.Rows.Count; i++)
					{
						string username = table.Rows[i][0].ToString();
						string password = table.Rows[i][1].ToString();
						string firstName = table.Rows[i][2].ToString();
						string middleName = table.Rows[i][3].ToString(); 
						string lastName = table.Rows[i][4].ToString();
						string vacancy = table.Rows[i][5].ToString();
						string email = table.Rows[i][6].ToString();
						testData.Add(new TestCaseData(username, password, firstName, middleName, lastName, vacancy, email));
					}
				}
			}
			return testData;
		}

		public static IEnumerable<TestCaseData> GetDeleteCandidateDatasFromExcel()
		{
			var testData = new List<TestCaseData>();
			using (var stream = File.Open("TestCaseData.xlsx", FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					var result = reader.AsDataSet();
					var table = result.Tables[6];
					for (int i = 1; i < table.Rows.Count; i++)
					{
						string username = table.Rows[i][0].ToString();
						string password = table.Rows[i][1].ToString();
						string candidateNo = table.Rows[i][2].ToString();
						testData.Add(new TestCaseData(username, password, candidateNo));
					}
				}
			}
			return testData;
		}
	}
}
