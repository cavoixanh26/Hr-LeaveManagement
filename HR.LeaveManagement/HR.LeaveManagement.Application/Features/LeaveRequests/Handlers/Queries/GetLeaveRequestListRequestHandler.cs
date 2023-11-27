using AutoMapper;
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
	public class GetLeaveRequestListRequestHandler : IRequestHandler<GetLeaveRequestListRequest, List<LeaveRequestListDto>>
	{
		private readonly ILeaveRequestRepository leaveRequestRepository;
		private readonly IMapper mapper;

		public GetLeaveRequestListRequestHandler(
			ILeaveRequestRepository leaveRequestRepository,
			IMapper mapper)
		{
			this.leaveRequestRepository = leaveRequestRepository;
			this.mapper = mapper;
		}
		public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
		{
			var leaveRequests = await leaveRequestRepository.GetLeaveRequestsWithDetailsAsync();
			var leaveRequestsMap = mapper.Map<List<LeaveRequestListDto>>(leaveRequests);

			return leaveRequestsMap;
		}
	}
}
