using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
	public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, int>
	{
		private readonly ILeaveRequestRepository leaveRequestRepository;
		private readonly IMapper mapper;

		public CreateLeaveRequestCommandHandler(
			ILeaveRequestRepository leaveRequestRepository,
			IMapper mapper)
		{
			this.leaveRequestRepository = leaveRequestRepository;
			this.mapper = mapper;
		}
		public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
		{
			var leaveRequest = mapper.Map<LeaveRequest>(request.LeaveRequestDto);
			leaveRequest = await leaveRequestRepository.AddAsync(leaveRequest);	
			return leaveRequest.Id;
		}
	}
}
