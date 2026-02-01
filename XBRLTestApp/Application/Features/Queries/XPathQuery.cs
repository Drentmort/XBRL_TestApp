using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLTestApp.Application.Features.Queries
{
	internal class XPathQuery : IRequest<string>
	{
		public string? QueryText { get; init; }
		public string? FileName { get; init; }
	}
}
