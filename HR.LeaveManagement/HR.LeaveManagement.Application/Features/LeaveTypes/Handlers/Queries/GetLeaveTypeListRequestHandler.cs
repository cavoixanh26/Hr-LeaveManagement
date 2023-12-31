﻿using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Queries
{
    public class GetLeaveTypeListRequestHandler : IRequestHandler<GetLeaveTypeListRequest, List<LeaveTypeDto>>
	{
		private readonly ILeaveTypeRepository leaveTypeRepository;
		private readonly IMapper mapper;

		public GetLeaveTypeListRequestHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
		{
			this.leaveTypeRepository = leaveTypeRepository;
			this.mapper = mapper;
		}

		public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypeListRequest request, CancellationToken cancellationToken)
		{
			var leaveTypes = await leaveTypeRepository.GetAllAsync();
			var leaveTypeMap = mapper.Map<List<LeaveTypeDto>>(leaveTypes);

			return leaveTypeMap;	
		}
	}
}
