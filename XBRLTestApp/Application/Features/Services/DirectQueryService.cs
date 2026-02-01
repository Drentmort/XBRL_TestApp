using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.Application.Interfaces;
using XBRLTestApp.Domain.Interfaces;

namespace XBRLTestApp.Application.Features.Services
{
	internal class DirectQueryService : IDirectQueryService
	{
		private readonly IInstanceRepositoryPool _instanceRepositoryPool;

		public DirectQueryService(IInstanceRepositoryPool instanceRepositoryPool)
		{
			_instanceRepositoryPool = instanceRepositoryPool;
		}

		public string ExecuteXPathQuery(string file, string request)
		{
			var repos = _instanceRepositoryPool.GetRepository(file);
			return repos.ExecuteXPathQuery(request);
		}
	}
}
