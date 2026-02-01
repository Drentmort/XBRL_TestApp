using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBRLTestApp.Application.Interfaces;

namespace XBRLTestApp.Application.Features.Queries
{
	internal class XPathQueryHandler : IRequestHandler<XPathQuery, string>
	{
		private readonly IDirectQueryService _directQueryService;

		public XPathQueryHandler(IDirectQueryService directQueryService)
		{
			_directQueryService = directQueryService;
		}

		public Task<string> Handle(XPathQuery request, CancellationToken cancellationToken)
		{
			return Task.FromResult(_directQueryService.ExecuteXPathQuery(request.FileName!, request.QueryText!));
		}
	}
}
