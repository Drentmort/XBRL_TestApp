using XBRLTestApp.Application.Common.Comparable;
using XBRLTestApp.Application.Extentions;
using XBRLTestApp.Application.Interfaces;
using XBRLTestApp.Domain.Entities;
using XBRLTestApp.Domain.Interfaces;

namespace XBRLTestApp.Application.Features.Services
{
	internal class ValidationService : IValidationService
	{
		private readonly IInstanceRepositoryPool _instanceRepositoryPool;

		public ValidationService(IInstanceRepositoryPool instanceRepositoryPool)
		{
			_instanceRepositoryPool = instanceRepositoryPool;
		}

		public bool HasDuplicated(string filePath, out IReadOnlyDictionary<ContextComparable, IReadOnlyList<Context>> duplicated)
		{
			var repository = _instanceRepositoryPool.GetRepository(filePath);
			var instance = repository.GetInstance();
			duplicated = instance.Contexts!
				.Select(x => (Original: x, Comparable: x.ToComparable()))
				.GroupBy(x => x.Comparable)
				.Where(x => x.Count() > 1)
				.ToDictionary(x => x.Key, x => (IReadOnlyList<Context>)x.Select(y => y.Original).ToList());

			return duplicated.Count > 0;
		}
	}
}
