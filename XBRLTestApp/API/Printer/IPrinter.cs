using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLTestApp.API.Printer
{
	internal interface IPrinter
	{
		void Clear();
		void Print(object data);
	}
}
