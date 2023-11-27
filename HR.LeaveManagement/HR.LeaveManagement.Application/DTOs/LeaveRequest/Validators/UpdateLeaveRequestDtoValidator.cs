using FluentValidation;
using HR.LeaveManagement.Application.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
	public class UpdateLeaveRequestDtoValidator : AbstractValidator<UpdateLeaveRequestDto>
	{
		private readonly ILeaveTypeRepository leaveTypeRepository;

		public UpdateLeaveRequestDtoValidator(ILeaveTypeRepository leaveTypeRepository)
		{
			this.leaveTypeRepository = leaveTypeRepository;
			Include(new ILeaveRequestDtoValidator(this.leaveTypeRepository));

			RuleFor(p => p.Id).NotNull().WithMessage("{PropertyName} must be present");
		}

	}
}
