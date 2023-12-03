using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repository
{
	public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
	{
		private readonly LeaveManagementDbContext context;
		public LeaveAllocationRepository(LeaveManagementDbContext context) 
			: base(context)
		{
			this.context = context;
		}

        public async Task AddAllocations(List<LeaveAllocation> allocations)
        {
			await context.LeaveAllocations.AddRangeAsync(allocations);
        }

        public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
        {
			var allocationExists = await context.LeaveAllocations
				.AnyAsync(q => q.EmployeeId == userId
				&& q.LeaveTypeId == leaveTypeId
				&& q.Period == period);

			return allocationExists;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetailsAsync()
		{
			var leaveAllocations = await context.LeaveAllocations
				.Include(x => x.LeaveType)
				.ToListAsync();

			return leaveAllocations;
		}

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetailsAsync(string userId)
        {
            var leaveAllocations = await context.LeaveAllocations
				.Where(q => q.EmployeeId == userId)
				.Include(q => q.LeaveType)
				.ToListAsync();
            return leaveAllocations;
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetailsAsync(int id)
		{
			var leaveAllocation = await context.LeaveAllocations
				.Include(x => x.LeaveType)
				.FirstOrDefaultAsync(x => x.Id == id);

			return leaveAllocation;
		}

        public async Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId)
        {
			var response = await context.LeaveAllocations
				.FirstOrDefaultAsync(q => q.EmployeeId == userId
				&& q.LeaveTypeId == leaveTypeId);	

			return response;
        }
    }
}
