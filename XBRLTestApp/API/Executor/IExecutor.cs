using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLTestApp.API.Executor
{
	internal interface IExecutor
	{
//		Задания:
//		- Найти ошибки в файле(повторяющиеся контексты).
		void FindReportFileErrors(string file);
//		- Объединить отчеты, на выходе получить новый объединенный отчет(xbrl) в объединными списками контекстов, единиц измерений и значений(фактов).
		void MergeReportFiles(string masterFile, string slaveFile, string outputFile);
//		- Выявить различия: список отсутствующих и новые факты, факты с различающимися значениями.
		void CalculateDifferenceInFactsOfReportFiles(string oldFile, string newFile);


//Написать запросы XPath для получения:
//- контексты с периодом xbrli:period/xbrli:instant, равным "2019-04-30";
		void RequestContextsWithInstant(string file, string dateTime);
//- контексты со сценарием, использующим измерение dimension="dim-int:ID_sobstv_CZBTaxis";
		void RequestContextsWithDimensions(string file, string dimention);
//- контексты без сценария;
		void RequestContextsWitrhNoScenatio(string file);
	}
}
