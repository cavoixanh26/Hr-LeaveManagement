using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
	public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, int>
	{
		private readonly ILeaveAllocationRepository leaveAllocationRepository;
		private readonly IMapper mapper;

		public CreateLeaveAllocationCommandHandler(
			ILeaveAllocationRepository leaveAllocationRepository,
			IMapper mapper)
		{
			this.leaveAllocationRepository = leaveAllocationRepository;
			this.mapper = mapper;
		}
		public async Task<int> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
		{
			var leaveAllocation = mapper.Map<LeaveAllocation>(request.LeaveAllocationDto);
			leaveAllocation = await leaveAllocationRepository.AddAsync(leaveAllocation);

			return leaveAllocation.Id;
		}
	}
}
