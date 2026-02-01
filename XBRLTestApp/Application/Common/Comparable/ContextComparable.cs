using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLTestApp.Application.Common.Comparable
{
	internal class ContextComparable
	{
		public string? EntityValue { get; init; }

		public string? EntityScheme { get; init; }

		public string? EntitySegment { get; init; }

		public DateTime? PeriodInstant { get; init; }

		public DateTime? PeriodStartDate { get; init; }

		public DateTime? PeriodEndDate { get; init; }
		public bool PeriodForever { get; init; }

		public List<ScenarioComparable> Scenarios { get; init; }

		public override bool Equals(object? obj)
		{
			return obj is ContextComparable comparable &&
				   EntityValue == comparable.EntityValue &&
				   EntityScheme == comparable.EntityScheme &&
				   EntitySegment == comparable.EntitySegment &&
				   PeriodInstant == comparable.PeriodInstant &&
				   PeriodStartDate == comparable.PeriodStartDate &&
				   PeriodEndDate == comparable.PeriodEndDate &&
				   PeriodForever == comparable.PeriodForever &&
				   Scenarios.SequenceEqual(comparable.Scenarios);
		}

		public override int GetHashCode()
		{
			var hashCode = new HashCode();
			hashCode.Add(EntityValue);
			hashCode.Add(EntityScheme);
			hashCode.Add(EntitySegment);
			hashCode.Add(PeriodInstant);
			hashCode.Add(PeriodStartDate);
			hashCode.Add(PeriodEndDate);
			hashCode.Add(PeriodForever);
			foreach (var scenario in Scenarios)
			{
				hashCode.Add(scenario.GetHashCode());
			}
			return hashCode.ToHashCode();
		}
	}
}
