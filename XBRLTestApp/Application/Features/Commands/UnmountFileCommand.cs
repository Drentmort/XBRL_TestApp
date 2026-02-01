using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLTestApp.Application.Features.Commands
{
	internal class UnmountFileCommand : IRequest
	{
		public string? FileName {  get; init; }
	}
}
