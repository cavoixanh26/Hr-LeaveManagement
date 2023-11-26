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
	public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand>
	{
		private readonly ILeaveRequestRepository leaveRequestRepository;
		private readonly IMapper mapper;

		public DeleteLeaveRequestCommandHandler(
			ILeaveRequestRepository leaveRequestRepository,
			IMapper mapper)
		{
			this.leaveRequestRepository = leaveRequestRepository;
			this.mapper = mapper;
		}
		public async Task Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
		{
			var leaveRequest = await leaveRequestRepository.GetAsync(request.Id);
			await leaveRequestRepository.DeleteAsync(leaveRequest);
		}
	}
}
