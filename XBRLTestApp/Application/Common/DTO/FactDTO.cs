using XBRLTestApp.Domain.Entities;

namespace XBRLTestApp.Application.Common.DTO
{
	internal class FactDTO
	{
		public string? Id { get; set; }
		public ContextDTO? Context { get; set; }
		public UnitDTO? Unit { get; set; }
		public int? Decimals { get; set; }
		public int? Precision { get; set; }
		public string? Value { get; set; }

		public static FactDTO MakeFromEntity(Fact fact)
		{
			return new FactDTO
			{
				Id = fact.Id,
				Context = fact.Context is not null ? ContextDTO.MakeFromEntity(fact.Context) : null!,
				Unit = fact.Unit is not null ? UnitDTO.MakeFromEntity(fact.Unit) : null!,
				Decimals = fact.Decimals,
				Precision = fact.Precision,
				Value = fact.Value
			};
		}
	}
}
