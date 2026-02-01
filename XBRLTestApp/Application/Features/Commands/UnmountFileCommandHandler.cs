using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.Domain.Interfaces;

namespace XBRLTestApp.Application.Features.Commands
{
	internal class UnmountFileCommandHandler : IRequestHandler<UnmountFileCommand>
	{
		private readonly IInstanceRepositoryPool _instanceRepositoryPool;

		public UnmountFileCommandHandler(IInstanceRepositoryPool instanceRepositoryPool)
		{
			_instanceRepositoryPool = instanceRepositoryPool;
		}

		public Task Handle(UnmountFileCommand request, CancellationToken cancellationToken)
		{
			_instanceRepositoryPool.UnregisterRepository(request.FileName!);

			return Task.CompletedTask;
		}
	}
}
