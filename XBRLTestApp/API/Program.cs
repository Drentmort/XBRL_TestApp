using Microsoft.Extensions.DependencyInjection;
using XBRLTestApp.API.Extentions;
using XBRLTestApp.API.Inputer;
using XBRLTestApp.Application.Extentions;
using XBRLTestApp.Application.Features;
using XBRLTestApp.Infrastructure.Extentions;
using XBRLTestApp.Infrastructure.ReportRepositories;

var serviceCollection = new ServiceCollection();

serviceCollection
	.AddApplicationLayer()
	.AddInfastructureLayer()
	.AddAPILayer();

var serviceProvider = serviceCollection.BuildServiceProvider();
var inputer = serviceProvider.GetRequiredService<IInputer>();
inputer.Run();

