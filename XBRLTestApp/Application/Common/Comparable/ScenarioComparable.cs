using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLTestApp.Application.Common.Comparable
{
	internal record ScenarioComparable
	{
		public string? DimensionType { get; init; } = default;

		public string? DimensionName { get; init; }

		public string? DimensionCode { get; init; }

		public string? DimensionValue { get; init; }
	}
}
