using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
	public class LeaveRequestService : ILeaveRequestService
	{
		public Task ApproveLeaveRequest(int id, bool approved)
		{
			throw new NotImplementedException();
		}

		public Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM leaveRequest)
		{
			throw new NotImplementedException();
		}

		public Task DeleteLeaveRequest(int id)
		{
			throw new NotImplementedException();
		}

		public Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList()
		{
			throw new NotImplementedException();
		}

		public Task<LeaveRequestVM> GetLeaveRequest(int id)
		{
			throw new NotImplementedException();
		}

		public Task<EmployeeLeaveRequestViewVM> GetUserLeaveRequests()
		{
			throw new NotImplementedException();
		}
	}
}
