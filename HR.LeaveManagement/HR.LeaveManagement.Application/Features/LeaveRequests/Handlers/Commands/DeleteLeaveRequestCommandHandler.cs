using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistences;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
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
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;

		public DeleteLeaveRequestCommandHandler(
            IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
		}
		public async Task Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
		{
			var leaveRequest = await unitOfWork.LeaveRequestRepository.GetAsync(request.Id);
			
			if (leaveRequest == null)
			{
				throw new NotFoundException(nameof(LeaveRequest), request.Id);
			}

			await unitOfWork.LeaveRequestRepository.DeleteAsync(leaveRequest);
			await unitOfWork.Save();
		}
	}
}
