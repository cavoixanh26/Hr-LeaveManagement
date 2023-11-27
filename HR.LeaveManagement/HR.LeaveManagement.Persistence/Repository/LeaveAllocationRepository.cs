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

		public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetailsAsync()
		{
			var leaveAllocations = await context.LeaveAllocations
				.Include(x => x.LeaveType)
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
	}
}
