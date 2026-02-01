using XBRLTestApp.API.Executor;
using XBRLTestApp.API.Printer;

namespace XBRLTestApp.API.Inputer
{
	internal class InputerImpl : IInputer, IDisposable
	{
		private readonly IPrinter _printer;
		private readonly IExecutor _executor;

		private readonly ManualResetEventSlim _disposedEvent = new(false);
		private readonly Thread _thread;
		private bool disposedValue;

		public InputerImpl(IExecutor executor, IPrinter printer)
		{
			_printer = printer;
			_executor = executor;
			_thread = new Thread(Process!);
			_thread.Name = "Input!";
		}

		private void Process(object _)
		{
			while (!_disposedEvent.IsSet)
			{
				try
				{
					_printer.Clear();
					_printer.Print("Enter case:");
					_printer.Print("1 - \"Найти ошибки в файле (повторяющиеся контексты).\"");
					_printer.Print("2 - \"Объединить отчеты, на выходе получить новый объединенный отчет (xbrl) в объединными списками контекстов, единиц измерений и значений (фактов).\"");
					_printer.Print("3 - \"Выявить различия: список отсутствующих и новые факты, факты с различающимися значениями.\"");
					_printer.Print("4 - \"Контексты с периодом xbrli:period/xbrli:instant, равным \"2019-04-30\";\"");
					_printer.Print("5 - \"Контексты со сценарием, использующим измерение dimension=\"dim-int:ID_sobstv_CZBTaxis\";\"");
					_printer.Print("6 - \"Контексты без сценария;\"");

					var result = Console.ReadLine();
					if (!int.TryParse(result, out var command) || command < 1 || command > 6)
					{
						_printer.Print("Wrong command entered. Please, enter number fron 1 to 6 as command.");
					}
					switch (command)
					{
						case 1:
							_printer.Print("Enter filename:");
							var filenameCmd1 = Console.ReadLine();

							_executor.FindReportFileErrors(filenameCmd1!);
							break;

						case 2:
							_printer.Print("Enter master report filename:");
							var masterFilenameCmd2 = Console.ReadLine();

							_printer.Print("Enter master report filename:");
							var slaveFilenameCmd2 = Console.ReadLine();

							_printer.Print("Enter master report filename:");
							var outpuFilenameCmd2 = Console.ReadLine();

							_executor.MergeReportFiles(masterFilenameCmd2!, slaveFilenameCmd2!, outpuFilenameCmd2!);
							break;

						case 3:
							_printer.Print("Enter old report filename:");
							var oldFilenameCmd3 = Console.ReadLine();

							_printer.Print("Enter new report filename:");
							var newFilenameCmd3 = Console.ReadLine();

							_executor.CalculateDifferenceInFactsOfReportFiles(oldFilenameCmd3!, newFilenameCmd3!);
							break;

						case 4:
							_printer.Print("Enter filename:");
							var filenameCmd4 = Console.ReadLine();

							_executor.RequestContextsWithInstant(filenameCmd4!, "2019-04-30");
							break;

						case 5:
							_printer.Print("Enter filename:");
							var filenameCmd5 = Console.ReadLine();

							_executor.RequestContextsWithDimensions(filenameCmd5!, "dim-int:ID_sobstv_CZBTaxis");
							break;

						case 6:
							_printer.Print("Enter filename:");
							var filenameCmd6 = Console.ReadLine();

							_executor.RequestContextsWitrhNoScenatio(filenameCmd6!);
							break;
					}

				}
				catch (Exception ex)
				{
					_printer.Print($"Exception occurred!!! -> {Environment.NewLine}{ex}");
				}

				_printer.Print("===================================================");
				_printer.Print("Enter to continue...");
				Console.ReadLine();
			}
		}

		public void Run()
		{
			_thread.Start();
		}

		private void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				_disposedEvent.Set();
				_thread.IsBackground = true;
				_thread.Priority = ThreadPriority.Lowest;

				disposedValue = true;
			}
		}

		~InputerImpl()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
