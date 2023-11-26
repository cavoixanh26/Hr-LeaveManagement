using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
	public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
	{
		private readonly ILeaveRequestRepository  leaveRequestRepository;
		private readonly IMapper mapper;

		public UpdateLeaveRequestCommandHandler(
			ILeaveRequestRepository leaveRequestRepository,
			IMapper mapper)
		{
			this.leaveRequestRepository = leaveRequestRepository;
			this.mapper = mapper;
		}
		public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
		{
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
