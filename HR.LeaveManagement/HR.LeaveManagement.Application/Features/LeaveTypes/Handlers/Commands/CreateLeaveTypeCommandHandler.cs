using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
	public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
	{
		private readonly ILeaveTypeRepository leaveTypeRepository;
		private readonly IMapper mapper;

		public CreateLeaveTypeCommandHandler(
			ILeaveTypeRepository leaveTypeRepository,
			IMapper mapper)
		{
			this.leaveTypeRepository = leaveTypeRepository;
			this.mapper = mapper;
		}

		public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
		{
			var validator = new CreateLeaveTypeDtoValidator();
			var validationResult = await validator.ValidateAsync(request.LeaveTypeDto);
			if (!validationResult.IsValid)
			{
				throw new Exception();
			}

			var leaveType = mapper.Map<LeaveType>(request.LeaveTypeDto);

			leaveType = await leaveTypeRepository.AddAsync(leaveType);

			return leaveType.Id;
		}
	}
}
