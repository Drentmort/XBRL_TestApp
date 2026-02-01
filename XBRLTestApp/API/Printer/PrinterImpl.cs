using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace XBRLTestApp.API.Printer
{
	internal class PrinterImpl : IPrinter
	{
		public void Clear()
		{
			Console.Clear();
		}

		public void Print(object data)
		{
			try
			{
				if(data is string)
				{
					Console.WriteLine(data);
				}
				else
				{
					Console.WriteLine(JsonSerializer.Serialize(data, new JsonSerializerOptions
					{
						WriteIndented = true,
					}));
				}

			}
			catch(Exception ex) 
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
