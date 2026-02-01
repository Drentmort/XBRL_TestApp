using XBRLTestApp.Domain.Entities;

namespace XBRLTestApp.Domain.Interfaces
{
	internal interface IInstanceRepository
	{
		Instance GetInstance();
		IReadOnlyDictionary<string, Context> GetIndexedContexts();
		IReadOnlyDictionary<string, Unit> GetIndexedUnits();
		IReadOnlyDictionary<string, Fact> GetIndexedFacts();
		string ExecuteXPathQuery(string request);
	}
}
