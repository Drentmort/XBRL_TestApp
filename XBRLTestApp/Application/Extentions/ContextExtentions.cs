using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.Application.Common.Comparable;
using XBRLTestApp.Domain.Entities;

namespace XBRLTestApp.Application.Extentions
{
	internal static class ContextExtentions
	{
		public static ContextComparable ToComparable(this Context context) =>
			 new ContextComparable
			 {
				 EntityScheme = context.EntityScheme,
				 EntitySegment = context.EntitySegment,
				 EntityValue = context.EntityValue,
				 PeriodEndDate = context.PeriodEndDate,
				 PeriodForever = context.PeriodForever,
				 PeriodInstant = context.PeriodInstant,
				 PeriodStartDate = context.PeriodStartDate,
				 Scenarios = context.Scenarios.Select(x => new ScenarioComparable
				 {
					 DimensionCode = x.DimensionCode,
					 DimensionName = x.DimensionName,
					 DimensionType = x.DimensionType,
					 DimensionValue = x.DimensionValue,
				 }).ToList()
			 };
	}
}
