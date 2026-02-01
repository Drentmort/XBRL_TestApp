using MediatR;
using XBRLTestApp.Application.Interfaces;

namespace XBRLTestApp.Application.Features.Commands
{
	internal class MergeFilesCommandHandler : IRequestHandler<MergeFilesCommand>
	{
		private readonly IMergeService _mergeService;

		public MergeFilesCommandHandler(IMergeService mergeService)
		{
			_mergeService = mergeService;
		}

		public Task Handle(MergeFilesCommand request, CancellationToken cancellationToken)
		{
			_mergeService.Merge(request.MasterFileName!, request.SlaveFileName!, request.OutputFileName!);
			return Task.CompletedTask;
		}
	}
}
