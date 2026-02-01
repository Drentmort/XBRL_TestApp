using MediatR;

namespace XBRLTestApp.Application.Features.Commands
{
	internal class MergeFilesCommand : IRequest
	{
		public string? MasterFileName { get; init; }
		public string? SlaveFileName { get; init; }
		public string? OutputFileName  { get; init; }
	}
}
