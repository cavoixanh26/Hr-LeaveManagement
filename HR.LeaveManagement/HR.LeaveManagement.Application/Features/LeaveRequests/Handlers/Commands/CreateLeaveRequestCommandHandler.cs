using AutoMapper;
using HR.LeaveManagement.Application.Constants;
using HR.LeaveManagement.Application.Contracts.Infrastructure;
using HR.LeaveManagement.Application.Contracts.Persistences;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
	public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
	{
		private readonly IUnitOfWork unitOfWork;
        private readonly IEmailSender emailSender;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
		public CreateLeaveRequestCommandHandler(
			IUnitOfWork unitOfWork,
			IEmailSender emailSender,
			IHttpContextAccessor httpContextAccessor,
			IMapper mapper)
		{
			this.unitOfWork = unitOfWork;
            this.emailSender = emailSender;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
		}
		public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseCommandResponse();
			// check validation request
			var validator = new CreateLeaveRequestDtoValidator(unitOfWork.LeaveTypeRepository);
			var validationResult = await validator.ValidateAsync(request.LeaveRequestDto);

			// get id employee who create request
			var userId = httpContextAccessor.HttpContext.User.Claims
				.FirstOrDefault(q => q.Type == CustomClaimTypes.Uid)?.Value;

			// get allocation of user
			var allocation = await unitOfWork.LeaveAllocationRepository
				.GetUserAllocations(userId, request.LeaveRequestDto.LeaveTypeId);

			// calculate date request of user 
			if (allocation is null)
			{
				validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure
					(nameof(request.LeaveRequestDto.LeaveTypeId), "You do not have any allocations for this leave type."));
			}else
			{
				var daysRequest = (int) (request.LeaveRequestDto.EndDate - request.LeaveRequestDto.StartDate).TotalDays;

				if (daysRequest > allocation.NumberOfDays)
				{
					validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
						nameof(request.LeaveRequestDto.EndDate), "You do not have enough days for this request"));
				}
			}

			if (!validationResult.IsValid)
			{
				response.Success = false;
				response.Message = "Request Failed";
				response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
			}else
			{
				// mapp from dto to entity
				var leaveRequest = mapper.Map<LeaveRequest>(request.LeaveRequestDto);
				leaveRequest.RequestingEmployeeId = userId;
				// add entity to db using repository pattern
				leaveRequest = await unitOfWork.LeaveRequestRepository.AddAsync(leaveRequest);
				await unitOfWork.Save();
				response.Success = true;
				response.Message = "Request Created Successfully";
				response.Id = leaveRequest.Id;

				try
				{
					// get email employee who create request
					var emailAddress = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email).Value;

					// handle send email
					var email = new Email
					{
						To = emailAddress,
						Body = $"Your leave request for {request.LeaveRequestDto.StartDate:D} " +
								$"to {request.LeaveRequestDto.EndDate:D} has been submitted successfully.",
						Subject = "Leave Request Submitted",
					};
					await emailSender.SendEmail(email);
				}
				catch (Exception ex)
				{

				}
			}

			return response;
		}
	}
}
