using MediatR;
using XBRLTestApp.Application.Common.DTO;

namespace XBRLTestApp.Application.Features.Queries
{
	internal class GetChangedFactsQuery : IRequest<IReadOnlyList<(FactDTO Old, FactDTO New)>>
	{
		public string? OldFileName { get; set; }
		public string? NewFileName { get; set; }
	}
}
