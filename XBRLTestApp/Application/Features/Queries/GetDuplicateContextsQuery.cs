using MediatR;
using XBRLTestApp.Application.Common.Comparable;
using XBRLTestApp.Application.Common.DTO;

namespace XBRLTestApp.Application.Features.Queries
{
	internal class GetDuplicateContextsQuery : IRequest<IReadOnlyDictionary<ContextComparable, IReadOnlyList<ContextDTO>>>
	{
		public string? FileName {  get; init; }
	}
}
