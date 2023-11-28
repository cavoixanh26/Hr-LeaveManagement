using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
	public class LeaveTypeService : ILeaveTypeService
	{
		public Task<Response<int>> CreateLeaveType(CreateLeaveTypeVM leaveType)
		{
			throw new NotImplementedException();
		}

		public Task<Response<int>> DeleteLeaveType(int id)
		{
			throw new NotImplementedException();
		}

		public Task<LeaveTypeVM> GetLeaveTypeDetails(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<LeaveTypeVM>> GetLeaveTypes()
		{
			throw new NotImplementedException();
		}

		public Task<Response<int>> UpdateLeaveType(int id, LeaveTypeVM leaveType)
		{
			throw new NotImplementedException();
		}
	}
}
