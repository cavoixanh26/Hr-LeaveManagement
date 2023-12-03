using AutoMapper;
using HR.LeaveManagement.Application.Constants;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
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
		private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;

        public GetLeaveRequestListRequestHandler(
			ILeaveRequestRepository leaveRequestRepository,
            IHttpContextAccessor httpContextAccessor,
			IUserService userService,
            IMapper mapper)
		{
			this.leaveRequestRepository = leaveRequestRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
            this.mapper = mapper;
		}
		public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
		{
			var leaveRequests = new List<LeaveRequest>();
			var requests = new List<LeaveRequestListDto>();

			if (request.IsLoggedInUser)
			{
				var userId = httpContextAccessor.HttpContext.User.FindFirst(
					q => q.Type == CustomClaimTypes.Uid)?.Value;
				leaveRequests = await leaveRequestRepository.GetLeaveRequestsWithDetailsAsync(userId);

				var employee = await userService.GetEmployee(userId);
				requests = mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
				requests.ForEach(x =>
				{
					x.Employee = employee;
				});
			}
			else
			{
				leaveRequests =  await leaveRequestRepository.GetLeaveRequestsWithDetailsAsync();
				requests = mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
                foreach (var req in requests)
                {
                    req.Employee = await userService.GetEmployee(req.RequestingEmployeeId);
                }
            }

			return requests;
		}
	}
}
