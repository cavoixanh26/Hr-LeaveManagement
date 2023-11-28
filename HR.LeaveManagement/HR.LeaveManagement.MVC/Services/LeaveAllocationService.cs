using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
	public class LeaveAllocationService : ILeaveAllocationService
	{
		public Task<Response<int>> CreateLeaveAllocations(int leaveTypeId)
		{
			throw new NotImplementedException();
		}
	}
}
