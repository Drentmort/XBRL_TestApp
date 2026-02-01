using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.Application.Common.DTO;
using XBRLTestApp.Application.Interfaces;
using XBRLTestApp.Domain.Entities;

namespace XBRLTestApp.Application.Features.Queries
{
	internal class GetChangedFactsQueryHandler : IRequestHandler<GetChangedFactsQuery, IReadOnlyList<(FactDTO Old, FactDTO New)>>
	{
		private readonly IDifferenceCalculationService _differenceCalculationService;

		public GetChangedFactsQueryHandler(IDifferenceCalculationService differenceCalculationService)
		{
			_differenceCalculationService = differenceCalculationService;
		}

		public Task<IReadOnlyList<(FactDTO Old, FactDTO New)>> Handle(GetChangedFactsQuery request, CancellationToken cancellationToken)
		{
			var changed = _differenceCalculationService.GetUpdated<Fact>(request.OldFileName, request.NewFileName, (fact1, fact2) =>
			{
				return 
					fact1.Value == fact2.Value &&
					fact1.Id == fact2.Id &&
					fact1.Decimals == fact2.Decimals &&
					fact1.ContextRef == fact2.ContextRef &&
					fact1.UnitRef == fact2.UnitRef &&
					fact1.Precision == fact2.Precision;
			});

			return Task.FromResult((IReadOnlyList<(FactDTO Old, FactDTO New)>)changed.Select(x => (Old: FactDTO.MakeFromEntity(x.Old), New: FactDTO.MakeFromEntity(x.New))).ToList());
		}
	}
}
