using HR.LeaveManagement.Application.Contracts.Persistences;
using HR.LeaveManagement.Application.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LeaveManagementDbContext context;
        private ILeaveAllocationRepository leaveAllocationRepository;
        private ILeaveRequestRepository leaveRequestRepository;
        private ILeaveTypeRepository leaveTypeRepository;

        public UnitOfWork(LeaveManagementDbContext context)
        {
            this.context = context;
        }

        public ILeaveAllocationRepository LeaveAllocationRepository 
            => leaveAllocationRepository ??= new LeaveAllocationRepository(context);

        public ILeaveRequestRepository LeaveRequestRepository 
            => leaveRequestRepository ??= new LeaveRequestRepository(context);

        public ILeaveTypeRepository LeaveTypeRepository 
            => leaveTypeRepository ??= new LeaveTypeRepository(context);

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
