using XBRLTestApp.Domain.Interfaces;
using XBRLTestApp.Infrastructure.Repositories;

namespace XBRLTestApp.Infrastructure.ReportRepositories
{
	internal class InstanceRepositoryPool : IInstanceRepositoryPool
	{
		private readonly Dictionary<string, IInstanceRepository> _repositories = new();

		public IInstanceRepository GetRepository(string filePath)
		{
			return _repositories.TryGetValue(filePath, out var repository) ? repository : default!;
		}

		public void RegisterRepository(string filePath)
		{
			UnregisterRepository(filePath);
			_repositories.Add(filePath, new InstanceRepository(filePath));
		}

		public void UnregisterRepository(string filePath)
		{
			if (_repositories.TryGetValue(filePath, out var repository))
			{
				_repositories.Remove(filePath);
			}
		}
	}
}
