using AutoMapper;
using HR.LeaveManagement.Application.Constants;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationListRequestHandler 
		: IRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDto>>
	{
		private readonly ILeaveAllocationRepository leaveAllocationRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;
        private readonly IMapper mapper;


		public GetLeaveAllocationListRequestHandler(
			ILeaveAllocationRepository leaveAllocationRepository,
			IHttpContextAccessor httpContextAccessor,
			IUserService userService,
			IMapper mapper)
		{
			this.leaveAllocationRepository = leaveAllocationRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
            this.mapper = mapper;
		}
		public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationListRequest request, CancellationToken cancellationToken)
		{
			var leaveAllocations = new List<LeaveAllocation>();
			var allocations = new List<LeaveAllocationDto>();

			if (request.IsLoggedInUser)
			{
				var userId = httpContextAccessor.HttpContext.User.FindFirst(
					q => q.Type == CustomClaimTypes.Uid)?.Value;
				leaveAllocations = await leaveAllocationRepository.GetLeaveAllocationsWithDetailsAsync(userId);

				var employee = await userService.GetEmployee(userId);
				allocations = mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
				allocations.ForEach(x =>
				{
					x.Employee = employee; 
				});
			}
			else
			{
				leaveAllocations = await leaveAllocationRepository.GetLeaveAllocationsWithDetailsAsync();
				allocations = mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
				allocations.ForEach(async x =>
				{
					x.Employee = await userService.GetEmployee(x.EmployeeId);
				});
			}

			return allocations;
		}
	}
}
