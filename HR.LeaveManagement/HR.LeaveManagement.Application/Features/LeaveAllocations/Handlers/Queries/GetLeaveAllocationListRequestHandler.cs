﻿using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;
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
		private readonly IMapper mapper;

		public GetLeaveAllocationListRequestHandler(
			ILeaveAllocationRepository leaveAllocationRepository,
			IMapper mapper)
		{
			this.leaveAllocationRepository = leaveAllocationRepository;
			this.mapper = mapper;
		}
		public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationListRequest request, CancellationToken cancellationToken)
		{
			var leaveAllocations = await leaveAllocationRepository.GetLeaveAllocationsWithDetails();
			var leaveAllocationsMap = mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);

			return leaveAllocationsMap;
		}
	}
}
