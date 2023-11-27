using FluentValidation;
using HR.LeaveManagement.Application.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators
{
	public class ILeaveAllocationDtoValidator : AbstractValidator<ILeaveAllocationDto>
	{
		private readonly ILeaveTypeRepository leaveTypeRepository;

		public ILeaveAllocationDtoValidator(ILeaveTypeRepository leaveTypeRepository)
		{
			this.leaveTypeRepository = leaveTypeRepository;

			RuleFor(p => p.NumberOfDays)
				.GreaterThan(0).WithMessage("{PropertyName} must greater than {ComparisonValue}");

			RuleFor(p => p.Period)
				.GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("{PropertyName} must be after {ComparisonValue}");

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
