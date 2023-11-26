using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
	public class UpdateLeaveAllocationCommandHandler 
		: IRequestHandler<UpdateLeaveAllocationCommand, Unit>
	{
		private readonly ILeaveAllocationRepository leaveAllocationRepository;
		private readonly IMapper mapper;

		public UpdateLeaveAllocationCommandHandler(
			ILeaveAllocationRepository leaveAllocationRepository,
			IMapper mapper)
		{
			this.leaveAllocationRepository = leaveAllocationRepository;
			this.mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateLeaveAllocationCommand request,
			CancellationToken cancellationToken)
		{
			var leaveAllocation = await leaveAllocationRepository
											.GetAsync(request.LeaveAllocationDto.Id);

			mapper.Map(leaveAllocation, request.LeaveAllocationDto);

			await leaveAllocationRepository.UpdateAsync(leaveAllocation);
			return Unit.Value;
		}
	}
}
