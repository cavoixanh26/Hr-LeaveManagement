using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistences;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
	public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand>
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;

		public DeleteLeaveTypeCommandHandler(
            IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
		}

		public async Task Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
		{
			var leaveType = await unitOfWork.LeaveTypeRepository.GetAsync(request.Id);
			
			if (leaveType == null)
			{
				throw new NotFoundException(nameof(LeaveType), request.Id);
			}

			await unitOfWork.LeaveTypeRepository.DeleteAsync(leaveType);
			await unitOfWork.Save();
		}
	}
}
