using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistences;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Exceptions;
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
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;

		public UpdateLeaveAllocationCommandHandler(
            IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateLeaveAllocationCommand request,
			CancellationToken cancellationToken)
		{
			// validator
			var validator = new UpdateLeaveAllocationDtoValidator(unitOfWork.LeaveTypeRepository);
			var validationResult = await validator.ValidateAsync(request.LeaveAllocationDto);

			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult);
			}

			var leaveAllocation = await unitOfWork.LeaveAllocationRepository
                                            .GetAsync(request.LeaveAllocationDto.Id);
			if (leaveAllocation is null)
				throw new NotFoundException(nameof(leaveAllocation), request.LeaveAllocationDto.Id);

			mapper.Map(leaveAllocation, request.LeaveAllocationDto);

			await unitOfWork.LeaveAllocationRepository.UpdateAsync(leaveAllocation);
			await unitOfWork.Save();
			return Unit.Value;
		}
	}
}
