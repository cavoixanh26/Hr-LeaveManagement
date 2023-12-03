using HR.LeaveManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Persistence.Contracts
{
	public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
	{
		Task<LeaveAllocation> GetLeaveAllocationWithDetailsAsync(int id);
		Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetailsAsync();
		Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetailsAsync(string userId);
		Task<bool> AllocationExists(string userId, int leaveTypeId, int period);
		Task AddAllocations(List<LeaveAllocation> allocations);	
		Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId);
	}
}
