using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLTestApp.Application.Common.DTO
{
	internal class ScenarioDTO
	{
		public string? DimensionType { get; set; } = default;
		public string? DimensionName { get; set; }
		public string? DimensionCode { get; set; }
		public string? DimensionValue { get; set; }
	}
}
