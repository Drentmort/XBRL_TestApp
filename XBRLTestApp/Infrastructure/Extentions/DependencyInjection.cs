using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.Application.Features.Services;
using XBRLTestApp.Application.Interfaces;
using XBRLTestApp.Domain.Interfaces;
using XBRLTestApp.Infrastructure.ReportRepositories;

namespace XBRLTestApp.Infrastructure.Extentions
{
	internal static class DependencyInjection
	{
		public static IServiceCollection AddInfastructureLayer(this IServiceCollection services)
		{
			services.AddSingleton<IInstanceRepositoryPool, InstanceRepositoryPool>();

			return services;
		}
	}
}
