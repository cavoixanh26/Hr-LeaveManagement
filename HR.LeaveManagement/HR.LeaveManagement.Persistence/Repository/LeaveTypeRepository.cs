using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Persistence.Repository
{
	public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
	{
		public LeaveTypeRepository(LeaveManagementDbContext context) : base(context)
		{
		}
	}
}
