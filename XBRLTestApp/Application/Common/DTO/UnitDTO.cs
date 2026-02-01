using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.Domain.Entities;

namespace XBRLTestApp.Application.Common.DTO
{
	internal class UnitDTO
	{
		public string? Id { get; set; }
		public string? Measure { get; set; }
		public string? Numerator { get; set; }
		public string? Denominator { get; set; }

		public static UnitDTO MakeFromEntity(Unit unit)
		{
			return new UnitDTO
			{
				Id = unit?.Id,
				Measure = unit?.Measure,
				Numerator = unit?.Numerator,
				Denominator = unit?.Denominator,
			};
		}
	}
}
