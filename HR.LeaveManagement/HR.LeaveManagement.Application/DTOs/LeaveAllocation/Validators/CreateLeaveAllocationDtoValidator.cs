using FluentValidation;
using HR.LeaveManagement.Application.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators
{
	public class CreateLeaveAllocationDtoValidator : AbstractValidator<CreateLeaveAllocationDto>
	{
		private readonly ILeaveTypeRepository leaveTypeRepository;

		public CreateLeaveAllocationDtoValidator(ILeaveTypeRepository leaveTypeRepository)
		{
			this.leaveTypeRepository = leaveTypeRepository;

			RuleFor(p => p.LeaveTypeId)
				.GreaterThan(0)
				.MustAsync(async (id, token) =>
				{
					var leaveTypeExists = await leaveTypeRepository.Exists(id);
					return leaveTypeExists;
				})
				.WithMessage("{PropertyName} does not exist.");
		}
	}
}
