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

namespace XBRLTestApp.Application.Extentions
{
	internal static class DependencyInjection
	{
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			services.AddTransient<IDirectQueryService, DirectQueryService>();
			services.AddTransient<IDifferenceCalculationService, DifferenceCalculationService>();
			services.AddTransient<IMergeService, MergeService>();
			services.AddTransient<IValidationService, ValidationService>();

			return services;
		}
	}
}
