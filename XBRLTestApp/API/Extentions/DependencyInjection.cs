using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.API.Executor;
using XBRLTestApp.API.Inputer;
using XBRLTestApp.API.Printer;

namespace XBRLTestApp.API.Extentions
{
	internal static class DependencyInjection
	{
		public static IServiceCollection AddAPILayer(this IServiceCollection services)
		{
			services.AddSingleton<IPrinter, PrinterImpl>();
			services.AddSingleton<IExecutor, ExecutorImpl>();
			services.AddSingleton<IInputer, InputerImpl>();

			return services;
		}
	}
}
