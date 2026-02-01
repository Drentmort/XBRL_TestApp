using XBRLTestApp.Domain.Entities;

namespace XBRLTestApp.Application.Interfaces
{
	internal interface IDifferenceCalculationService
	{
		IReadOnlyList<T> GetAdded<T>(string fileOld, string fileNew);
		IReadOnlyList<T> GetRemoved<T>(string fileOld, string fileNew);
		IReadOnlyList<(T Old, T New)> GetUpdated<T>(string fileOld, string fileNew, Func<T, T, bool> compareFunc);
		IReadOnlyList<(T Old, T New)> GetUnchanged<T>(string fileOld, string fileNew, Func<T, T, bool> compareFunc);
	}
}
