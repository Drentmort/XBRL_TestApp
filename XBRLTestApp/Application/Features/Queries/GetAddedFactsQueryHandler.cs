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
	internal class GetAddedFactsQueryHandler : IRequestHandler<GetAddedFactsQuery, IReadOnlyList<FactDTO>>
	{
		private readonly IDifferenceCalculationService _differenceCalculationService;

		public GetAddedFactsQueryHandler(IDifferenceCalculationService differenceCalculationService)
		{
			_differenceCalculationService = differenceCalculationService;
		}

		public Task<IReadOnlyList<FactDTO>> Handle(GetAddedFactsQuery request, CancellationToken cancellationToken)
		{
			var added = _differenceCalculationService.GetAdded<Fact>(request.OldFileName!, request.NewFileName!);
			return Task.FromResult((IReadOnlyList<FactDTO>)added.Select(x => FactDTO.MakeFromEntity(x)));
		}
	}
}
