using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistences;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
	public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand>
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;

		public DeleteLeaveAllocationCommandHandler(
            IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
		}
		public async Task Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
		{
			var leaveAllocation = await unitOfWork.LeaveAllocationRepository.GetAsync(request.Id);

			if (leaveAllocation == null)
			{
				throw new NotFoundException(nameof(LeaveAllocation), request.Id);
			}

			await unitOfWork.LeaveAllocationRepository.DeleteAsync(leaveAllocation);
			await unitOfWork.Save();
		}
	}
}
