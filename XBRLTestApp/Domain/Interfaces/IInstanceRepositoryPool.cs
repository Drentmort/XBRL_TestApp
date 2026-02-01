using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLTestApp.Domain.Interfaces
{
	internal interface IInstanceRepositoryPool
	{
		IInstanceRepository GetRepository(string filePath);
		void RegisterRepository (string filePath);
		void UnregisterRepository (string filePath);
	}
}
