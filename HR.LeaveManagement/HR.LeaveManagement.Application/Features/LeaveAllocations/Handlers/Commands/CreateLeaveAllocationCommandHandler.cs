using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
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
        private readonly IUserService userService;
        private readonly IMapper mapper;

		public CreateLeaveAllocationCommandHandler(
			ILeaveAllocationRepository leaveAllocationRepository, 
			ILeaveTypeRepository leaveTypeRepository,
			IUserService userService,
			IMapper mapper)
		{
			this.leaveAllocationRepository = leaveAllocationRepository;
			this.leaveTypeRepository = leaveTypeRepository;
            this.userService = userService;
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
				var leaveType = await leaveTypeRepository.GetAsync(request.LeaveAllocationDto.LeaveTypeId);
				var employees = await userService.GetEmployees();
				var period = DateTime.Now.Year;
				var allocations = new List<LeaveAllocation>();

				foreach (var employee in employees)
				{
					var allocationExist = await leaveAllocationRepository.AllocationExists(employee.Id, leaveType.Id, period);
					if (!allocationExist)
					{
						allocations.Add(new LeaveAllocation
						{
							EmployeeId = employee.Id,
							LeaveTypeId = leaveType.Id,
							NumberOfDays = leaveType.DefaultDays,
							Period = period
						});
					}
				}

				await leaveAllocationRepository.AddAllocations(allocations);

				response.Success = true;
				response.Message = "Allocations Successful";
			}

			return response;
		}
	}
}
