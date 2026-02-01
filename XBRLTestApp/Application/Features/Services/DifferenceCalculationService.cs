using XBRLTestApp.Application.Interfaces;
using XBRLTestApp.Domain.Entities;
using XBRLTestApp.Domain.Interfaces;

namespace XBRLTestApp.Application.Features.Services
{
	internal class DifferenceCalculationService : IDifferenceCalculationService
	{
		private IInstanceRepositoryPool _instanceRepositoryPool;

		public DifferenceCalculationService(IInstanceRepositoryPool instanceRepositoryPool)
		{
			_instanceRepositoryPool = instanceRepositoryPool;
		}

		public IReadOnlyList<T> GetAdded<T>(string fileOld, string fileNew) 
		{
			var contextsOld = GetIndexedEntities<T>(fileOld);
			var contextsNew = GetIndexedEntities<T>(fileNew);
			return contextsNew.Where(x => !contextsOld.ContainsKey(x.Key)).Select(x => x.Value).ToList();
		}

		public IReadOnlyList<T> GetRemoved<T>(string fileOld, string fileNew)
		{
			var contextsOld = GetIndexedEntities<T>(fileOld);
			var contextsNew = GetIndexedEntities<T>(fileNew);
			return contextsNew.Where(x => !contextsOld.ContainsKey(x.Key)).Select(x => x.Value).ToList();
		}

		public IReadOnlyList<(T Old, T New)> GetUpdated<T>(string fileOld, string fileNew, Func<T, T, bool> compareFunc)
		{
			var contextsOld = GetIndexedEntities<T>(fileOld);
			var contextsNew = GetIndexedEntities<T>(fileNew);

			var intersectedKeys = contextsNew.Keys.IntersectBy(contextsOld.Keys, x => x);

			var result = new List<(T Old, T New)>();

			foreach (var key in intersectedKeys)
			{
				if (contextsNew.TryGetValue(key, out var contextNew) && contextsOld.TryGetValue(key, out var contextOld))
				{
					if (compareFunc(contextNew, contextOld))
					{
						result.Add((Old: contextOld, New: contextNew));
					}
				}
			}
			return result;
		}

		public IReadOnlyList<(T Old, T New)> GetUnchanged<T>(string fileOld, string fileNew, Func<T, T, bool> compareFunc)
		{
			var contextsOld = GetIndexedEntities<T>(fileOld);
			var contextsNew = GetIndexedEntities<T>(fileNew);

			var intersectedKeys = contextsNew.Keys.IntersectBy(contextsOld.Keys, x => x);

			var result = new List<(T Old, T New)>();

			foreach (var key in intersectedKeys)
			{
				if (contextsNew.TryGetValue(key, out var contextNew) && contextsOld.TryGetValue(key, out var contextOld))
				{
					if (compareFunc(contextNew,contextOld))
					{
						result.Add((Old: contextOld, New: contextNew));
					}
				}
			}
			return result;
		}

		private IReadOnlyDictionary<string, T> GetIndexedEntities<T>(string file)
		{
			var repos = _instanceRepositoryPool.GetRepository(file);
			if(typeof(T) == typeof(Context))
			{
				return (IReadOnlyDictionary<string, T>)repos.GetIndexedContexts();
			}
			if (typeof(T) == typeof(Unit))
			{
				return (IReadOnlyDictionary<string, T>)repos.GetIndexedUnits();
			}
			if (typeof(T) == typeof(Fact))
			{
				return (IReadOnlyDictionary<string, T>)repos.GetIndexedFacts();
			}
			throw new NotImplementedException();
		}
	}
}
