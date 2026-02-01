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
	internal class GetRemovedFactsQueryHandler : IRequestHandler<GetRemovedFactsQuery, IReadOnlyList<FactDTO>>
	{
		private readonly IDifferenceCalculationService _differenceCalculationService;

		public GetRemovedFactsQueryHandler(IDifferenceCalculationService differenceCalculationService)
		{
			_differenceCalculationService = differenceCalculationService;
		}

		public Task<IReadOnlyList<FactDTO>> Handle(GetRemovedFactsQuery request, CancellationToken cancellationToken)
		{
			var removed = _differenceCalculationService.GetRemoved<Fact>(request.OldFileName!, request.NewFileName!);
			return Task.FromResult((IReadOnlyList<FactDTO>)removed.Select(x => FactDTO.MakeFromEntity(x)));
		}
	}
}
