using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.API.Printer;
using XBRLTestApp.Application.Features.Commands;
using XBRLTestApp.Application.Features.Queries;

namespace XBRLTestApp.API.Executor
{
	internal class ExecutorImpl : IExecutor
	{
		private readonly IMediator _mediator;
		private readonly IPrinter _printer;

		public ExecutorImpl(IMediator mediator, IPrinter printer)
		{
			_mediator = mediator;
			_printer = printer;
		}

		public void CalculateDifferenceInFactsOfReportFiles(string oldFile, string newFile)
		{
			_mediator.Send(new MountFileCommand { FileName = oldFile });
			_mediator.Send(new MountFileCommand { FileName = newFile });

			var added = _mediator.Send(new GetAddedFactsQuery 
			{ 
				OldFileName = oldFile, 
				NewFileName = newFile 
			}).Result;

			_printer.Print("Added:");
			_printer.Print(added);

			var removed = _mediator.Send(new GetRemovedFactsQuery
			{
				OldFileName = oldFile,
				NewFileName = newFile
			}).Result;

			_printer.Print(Environment.NewLine);
			_printer.Print("Removed:");
			_printer.Print(removed);


			var changed = _mediator.Send(new GetChangedFactsQuery
			{
				OldFileName = oldFile,
				NewFileName = newFile
			}).Result;

			_printer.Print(Environment.NewLine);
			_printer.Print("Changed:");
			foreach (var change in changed)
			{
				_printer.Print(change.Old);
				_printer.Print(Environment.NewLine);
				_printer.Print("Turns to:");
				_printer.Print(Environment.NewLine);
				_printer.Print(change.New);
			}
			_mediator.Send(new UnmountFileCommand { FileName = oldFile });
			_mediator.Send(new UnmountFileCommand { FileName = newFile });
		}

		public void FindReportFileErrors(string file)
		{
			_mediator.Send(new MountFileCommand { FileName = file });

			var duplicates = _mediator.Send(new GetDuplicateContextsQuery
			{
				FileName = file
			}).Result;

			_printer.Print("Duplicates found:");
			foreach (var duplicate in duplicates)
			{
				_printer.Print(duplicate);
			}

			_mediator.Send(new UnmountFileCommand { FileName = file });
		}

		public void MergeReportFiles(string masterFile, string slaveFile, string outputFile)
		{
			_mediator.Send(new MountFileCommand { FileName = masterFile });
			_mediator.Send(new MountFileCommand { FileName = slaveFile });

			_printer.Print($"Execute merge {slaveFile} to {masterFile}");
			_mediator.Send(new MergeFilesCommand
			{
				MasterFileName = masterFile,
				SlaveFileName = slaveFile,
				OutputFileName = outputFile
			});

			_mediator.Send(new UnmountFileCommand { FileName = masterFile });
			_mediator.Send(new UnmountFileCommand { FileName = slaveFile });

		}

		public void RequestContextsWithDimensions(string file, string dimention)
		{
			_mediator.Send(new MountFileCommand { FileName = file });

			_printer.Print($"Execute query //xbrli:context[xbrli:scenario/xbrldi:typedMember[@dimension='{dimention}']]");

			var result = _mediator.Send(new XPathQuery 
			{
				FileName = file , 
				QueryText = $"//xbrli:context[xbrli:scenario/xbrldi:typedMember[@dimension='{dimention}']]" 
			}).Result;

			_printer.Print("Result:");
			_printer.Print(result);

			_mediator.Send(new UnmountFileCommand { FileName = file });
		}

		public void RequestContextsWithInstant(string file, string dateTime)
		{
			_mediator.Send(new MountFileCommand { FileName = file });

			_printer.Print($"Execute query //xbrli:context[xbrli:period/xbrli:instant = '{dateTime}']");

			var result = _mediator.Send(new XPathQuery
			{
				FileName = file,
				QueryText = $"//xbrli:context[xbrli:period/xbrli:instant = '{dateTime}']"
			}).Result;

			_printer.Print("Result:");
			_printer.Print(result);

			_mediator.Send(new UnmountFileCommand { FileName = file });
		}

		public void RequestContextsWitrhNoScenatio(string file)
		{
			_mediator.Send(new MountFileCommand { FileName = file });

			_printer.Print($"Execute query //xbrli:context[not(xbrli:scenario)]");

			var result = _mediator.Send(new XPathQuery
			{
				FileName = file,
				QueryText = $"//xbrli:context[not(xbrli:scenario)]"
			}).Result;

			_printer.Print("Result:");
			_printer.Print(result);

			_mediator.Send(new UnmountFileCommand { FileName = file });
		}
	}
}
