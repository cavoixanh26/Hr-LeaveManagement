using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistences;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
	public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Unit>
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;

		public UpdateLeaveTypeCommandHandler(
            IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
		{
			// validator
			var validator = new UpdateLeaveTypeDtoValidator();
			var validationResult = await validator.ValidateAsync(request.LeaveTypeDto);

			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult);
			}

			var leaveType = await unitOfWork.LeaveTypeRepository.GetAsync(request.LeaveTypeDto.Id);

			if (leaveType is null)
				throw new NotFoundException(nameof(leaveType), request.LeaveTypeDto.Id);

			mapper.Map(request.LeaveTypeDto, leaveType);

			await unitOfWork.LeaveTypeRepository.UpdateAsync(leaveType);
			await unitOfWork.Save();
			return Unit.Value;
		}
	}
}
