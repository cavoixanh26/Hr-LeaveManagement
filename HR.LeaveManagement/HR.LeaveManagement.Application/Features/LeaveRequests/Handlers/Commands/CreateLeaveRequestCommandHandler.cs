using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Infrastructure;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
	public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
	{
		private readonly ILeaveRequestRepository leaveRequestRepository;
		private readonly IEmailSender emailSender;
		private readonly IMapper mapper;
		private readonly ILeaveTypeRepository leaveTypeRepository;
		public CreateLeaveRequestCommandHandler(
			ILeaveTypeRepository leaveTypeRepository,
			ILeaveRequestRepository leaveRequestRepository,
			IEmailSender emailSender,
			IMapper mapper)
		{
			this.leaveTypeRepository = leaveTypeRepository;
			this.leaveRequestRepository = leaveRequestRepository;
			this.emailSender = emailSender;
			this.mapper = mapper;
		}
		public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseCommandResponse();
			// check validation request
			var validator = new CreateLeaveRequestDtoValidator(leaveTypeRepository);
			var validationResult = await validator.ValidateAsync(request.LeaveRequestDto);

			if (!validationResult.IsValid)
			{
				response.Success = false;
				response.Message = "Request Failed";
				response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
			}else
			{
				// mapp from dto to entity
				var leaveRequest = mapper.Map<LeaveRequest>(request.LeaveRequestDto);
				// add entity to db using repository pattern
				leaveRequest = await leaveRequestRepository.AddAsync(leaveRequest);

				response.Success = true;
				response.Message = "Request Created Successfully";
				response.Id = leaveRequest.Id;

				// handle send email
				var email = new Email
				{
					To = "employee@gmail.com",
					Body = $"Your leave request for {request.LeaveRequestDto.StartDate:D} to {request.LeaveRequestDto.EndDate:D} has been submitted successfully.",
					Subject = "Leave Request Submitted",
				};
				try
				{
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
