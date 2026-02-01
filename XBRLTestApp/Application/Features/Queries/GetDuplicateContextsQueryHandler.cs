using MediatR;
using XBRLTestApp.Application.Common.Comparable;
using XBRLTestApp.Application.Common.DTO;
using XBRLTestApp.Application.Interfaces;

namespace XBRLTestApp.Application.Features.Queries
{
	internal class GetDuplicateContextsQueryHandler : IRequestHandler<GetDuplicateContextsQuery, IReadOnlyDictionary<ContextComparable, IReadOnlyList<ContextDTO>>>
	{
		private readonly IValidationService _validationService;

		public GetDuplicateContextsQueryHandler(IValidationService validationService)
		{
			_validationService = validationService;
		}

		public Task<IReadOnlyDictionary<ContextComparable, IReadOnlyList<ContextDTO>>> Handle(GetDuplicateContextsQuery request, CancellationToken cancellationToken)
		{
			if(_validationService.HasDuplicated(request.FileName!, out var duplicated))
			{
				var converted = duplicated.ToDictionary(
					x => x.Key, 
					x => (IReadOnlyList<ContextDTO>)x.Value
						.Select(y => ContextDTO.MakeFromEntity(y))
						.ToList());

				return Task.FromResult((IReadOnlyDictionary<ContextComparable, IReadOnlyList<ContextDTO>>)converted);
			}
			return Task.FromResult((IReadOnlyDictionary<ContextComparable, IReadOnlyList<ContextDTO>>)new Dictionary<ContextComparable, IReadOnlyList<ContextDTO>>());
		}
	}
}
