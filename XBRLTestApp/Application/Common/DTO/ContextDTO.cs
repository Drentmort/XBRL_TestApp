using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.Domain.Entities;

namespace XBRLTestApp.Application.Common.DTO
{
	internal record ContextDTO
	{
		public string? Id { get; set; }
		public string? EntityValue { get; set; }
		public string? EntityScheme { get; set; }
		public string? EntitySegment { get; set; }
		public DateTime? PeriodInstant { get; set; }
		public DateTime? PeriodStartDate { get; set; }
		public DateTime? PeriodEndDate { get; set; }
		public bool PeriodForever { get; set; }
		public List<ScenarioDTO> Scenarios { get; set; } = new();


		public static ContextDTO MakeFromEntity(Context context)
		{
			return new ContextDTO
			{
				EntityScheme = context.EntityScheme,
				EntitySegment = context.EntitySegment,
				EntityValue = context.EntityValue,
				Id = context.Id,
				PeriodEndDate = context.PeriodEndDate,
				PeriodForever = context.PeriodForever,
				PeriodInstant = context.PeriodInstant,
				PeriodStartDate = context.PeriodStartDate,
				Scenarios = context.Scenarios.Select(z => new ScenarioDTO
				{
					DimensionCode = z.DimensionCode,
					DimensionName = z.DimensionName,
					DimensionType = z.DimensionType,
					DimensionValue = z.DimensionValue,
				}).ToList()
			};
		}

	}
}
