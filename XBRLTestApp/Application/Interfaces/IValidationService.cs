using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.Application.Common.Comparable;
using XBRLTestApp.Domain.Entities;

namespace XBRLTestApp.Application.Interfaces
{
	internal interface IValidationService
	{
		bool HasDuplicated(string filePath, out IReadOnlyDictionary<ContextComparable, IReadOnlyList<Context>> duplicated);
	}

}
