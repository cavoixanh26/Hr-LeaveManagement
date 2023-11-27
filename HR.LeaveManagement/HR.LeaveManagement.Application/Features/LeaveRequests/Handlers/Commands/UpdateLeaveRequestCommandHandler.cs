using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
	public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
	{
		private readonly ILeaveRequestRepository  leaveRequestRepository;
		private readonly ILeaveTypeRepository leaveTypeRepository;
		private readonly IMapper mapper;

		public UpdateLeaveRequestCommandHandler(
			ILeaveRequestRepository leaveRequestRepository,
			ILeaveTypeRepository leaveTypeRepository,
			IMapper mapper)
		{
			this.leaveRequestRepository = leaveRequestRepository;
			this.leaveTypeRepository = leaveTypeRepository;
			this.mapper = mapper;
		}
		public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
		{
			// validator
			var validator = new UpdateLeaveRequestDtoValidator(leaveTypeRepository);
			var validationResult = await validator.ValidateAsync(request.LeaveRequestDto);

			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult);
			}

			var leaveRequest = await leaveRequestRepository.GetAsync(request.Id);

			if (request.LeaveRequestDto != null)
			{
				mapper.Map(leaveRequest, request.LeaveRequestDto);

				await leaveRequestRepository.UpdateAsync(leaveRequest);
			}
			else if (request.ChangeLeaveRequestApprovalDto != null) 
			{
				await leaveRequestRepository
					.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);
			}

			return Unit.Value;
		}
	}
}
