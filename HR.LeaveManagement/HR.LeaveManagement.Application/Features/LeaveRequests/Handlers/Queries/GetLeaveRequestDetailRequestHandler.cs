using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries
{
	public class GetLeaveRequestDetailRequestHandler : IRequestHandler<GetLeaveRequestDetailRequest, LeaveRequestDto>
	{
		private readonly ILeaveRequestRepository leaveRequestRepository;
        private readonly IUserService userService;
        private readonly IMapper mapper;

		public GetLeaveRequestDetailRequestHandler(
			ILeaveRequestRepository leaveRequestRepository,
			IUserService userService,
			IMapper mapper)
		{
			this.leaveRequestRepository = leaveRequestRepository;
            this.userService = userService;
            this.mapper = mapper;
		}

		public async Task<LeaveRequestDto> Handle(GetLeaveRequestDetailRequest request, CancellationToken cancellationToken)
		{
			var leaveRequest = await leaveRequestRepository.GetLeaveRequestWithDetailsAsync(request.Id);
			var leaveRequestMap = mapper.Map<LeaveRequestDto>(leaveRequest);
			leaveRequestMap.Employee = await userService.GetEmployee(leaveRequest.RequestingEmployeeId);
			leaveRequestMap.DateRequested = leaveRequest.DateCreated;
			return leaveRequestMap;	
		}
	}
}
