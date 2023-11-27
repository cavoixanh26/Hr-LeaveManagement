using AutoMapper;
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
		private readonly ILeaveTypeRepository leaveTypeRepository;
		private readonly IMapper mapper;

		public UpdateLeaveTypeCommandHandler(
			ILeaveTypeRepository leaveTypeRepository,
			IMapper mapper)
		{
			this.leaveTypeRepository = leaveTypeRepository;
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

			var leaveType = await leaveTypeRepository.GetAsync(request.LeaveTypeDto.Id);

			mapper.Map(leaveType, request.LeaveTypeDto);

			await leaveTypeRepository.UpdateAsync(leaveType);
			return Unit.Value;
		}
	}
}
