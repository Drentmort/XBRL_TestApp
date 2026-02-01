using MediatR;
using XBRLTestApp.Application.Common.DTO;

namespace XBRLTestApp.Application.Features.Queries
{
	internal class GetAddedFactsQuery : IRequest<IReadOnlyList<FactDTO>>
	{
		public string? OldFileName { get; set; }
		public string? NewFileName { get; set; }
	}
}
