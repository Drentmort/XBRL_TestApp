using Microsoft.Extensions.DependencyInjection;
using XBRLTestApp.API.Extentions;
using XBRLTestApp.API.Inputer;
using XBRLTestApp.Application.Extentions;
using XBRLTestApp.Application.Features;
using XBRLTestApp.Infrastructure.Extentions;
using XBRLTestApp.Infrastructure.ReportRepositories;

Console.WriteLine("Hello, World!");

//var pool = new InstanceRepositoryPool();
//pool.RegisterRepository("E:\\Загрузки\\report3 .xbrl");
//var validationService = new ValidationService(pool);

//var hasDuplicates = validationService.HasDuplicated("E:\\Загрузки\\report3 .xbrl", out var duplicates);

//pool.RegisterRepository("E:\\Загрузки\\report1.xbrl");
//pool.RegisterRepository("E:\\Загрузки\\report2.xbrl");

//var differenceCalculationService = new DifferenceCalculationService(pool);
//var mergeService = new MergeService(pool);

//var added = differenceCalculationService.GetAdded("E:\\Загрузки\\report1.xbrl", "E:\\Загрузки\\report2.xbrl");
//var removed = differenceCalculationService.GetRemoved("E:\\Загрузки\\report1.xbrl", "E:\\Загрузки\\report2.xbrl");
//var updated = differenceCalculationService.GetUpdated("E:\\Загрузки\\report1.xbrl", "E:\\Загрузки\\report2.xbrl");
//var unchanged = differenceCalculationService.GetUpchanged("E:\\Загрузки\\report1.xbrl", "E:\\Загрузки\\report2.xbrl");

//mergeService.Merge("E:\\Загрузки\\report1.xbrl", "E:\\Загрузки\\report2.xbrl", "E:\\Загрузки\\report123.xbrl");

var serviceCollection = new ServiceCollection();

serviceCollection
	.AddApplicationLayer()
	.AddInfastructureLayer()
	.AddAPILayer();

var serviceProvider = serviceCollection.BuildServiceProvider();
var inputer = serviceProvider.GetRequiredService<IInputer>();
inputer.Run();
