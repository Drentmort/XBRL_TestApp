using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.Domain.Interfaces;

namespace XBRLTestApp.Application.Features.Commands
{
	internal class MountFileCommandHandler : IRequestHandler<MountFileCommand>
	{
		private readonly IInstanceRepositoryPool _instanceRepositoryPool;

		public MountFileCommandHandler(IInstanceRepositoryPool instanceRepositoryPool)
		{
			_instanceRepositoryPool = instanceRepositoryPool;
		}

		public Task Handle(MountFileCommand request, CancellationToken cancellationToken)
		{
			_instanceRepositoryPool.RegisterRepository(request.FileName!);

			return Task.CompletedTask;
		}
	}
}
