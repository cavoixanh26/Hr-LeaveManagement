using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistences;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UpdateLeaveRequestCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork= unitOfWork;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await unitOfWork.LeaveRequestRepository.GetAsync(request.Id);
            if (leaveRequest is null)
                throw new NotFoundException(nameof(leaveRequest), request.Id);

            if (request.LeaveRequestDto != null)
            {
                // validator
                var validator = new UpdateLeaveRequestDtoValidator(unitOfWork.LeaveTypeRepository);
                var validationResult = await validator.ValidateAsync(request.LeaveRequestDto);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult);
                }
                mapper.Map(leaveRequest, request.LeaveRequestDto);

                await unitOfWork.LeaveRequestRepository.UpdateAsync(leaveRequest);
                await unitOfWork.Save();
            }
            else if (request.ChangeLeaveRequestApprovalDto != null)
            {
                await unitOfWork.LeaveRequestRepository
                    .ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);
                if (request.ChangeLeaveRequestApprovalDto.Approved.HasValue
                    && request.ChangeLeaveRequestApprovalDto.Approved.Value == true)
                {
                    var allocation = await unitOfWork.LeaveAllocationRepository
                        .GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);

                    int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;

                    allocation.NumberOfDays -= daysRequested;
                    await unitOfWork.LeaveAllocationRepository.UpdateAsync(allocation);
                }
                await unitOfWork.Save();
            }

            return Unit.Value;
        }
    }
}
