﻿using FluentValidation;
using HR.LeaveManagement.Application.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
	public class ILeaveRequestDtoValidator : AbstractValidator<ILeaveRequestDto>
	{
		private readonly ILeaveTypeRepository leaveTypeRepository;
		public ILeaveRequestDtoValidator(ILeaveTypeRepository leaveTypeRepository) 
		{
			this.leaveTypeRepository = leaveTypeRepository;

			Include(new ILeaveRequestDtoValidator(leaveTypeRepository));
		}
	}
}
