using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
	public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse>
	{
		private readonly ILeaveAllocationRepository leaveAllocationRepository;
		private readonly ILeaveTypeRepository leaveTypeRepository;
		private readonly IMapper mapper;

		public CreateLeaveAllocationCommandHandler(
			ILeaveAllocationRepository leaveAllocationRepository, 
			ILeaveTypeRepository leaveTypeRepository,
			IMapper mapper)
		{
			this.leaveAllocationRepository = leaveAllocationRepository;
			this.leaveTypeRepository = leaveTypeRepository;
			this.mapper = mapper;
		}
		public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseCommandResponse();
			var validator = new CreateLeaveAllocationDtoValidator(leaveTypeRepository);
			var validationResult = await validator.ValidateAsync(request.LeaveAllocationDto);

			if (!validationResult.IsValid)
			{
				response.Success = false;
				response.Message = "Allocations Failed";
				response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
			}else
			{
				var leaveAllocation = mapper.Map<LeaveAllocation>(request.LeaveAllocationDto);
				leaveAllocation = await leaveAllocationRepository.AddAsync(leaveAllocation);

				response.Success = true;
				response.Message = "Allocations Successful";
			}


			return response;
		}
	}
}
