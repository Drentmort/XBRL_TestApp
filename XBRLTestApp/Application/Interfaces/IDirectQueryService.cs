using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLTestApp.Application.Interfaces
{
	internal interface IDirectQueryService
	{
		string ExecuteXPathQuery(string file, string request);
	}
}
