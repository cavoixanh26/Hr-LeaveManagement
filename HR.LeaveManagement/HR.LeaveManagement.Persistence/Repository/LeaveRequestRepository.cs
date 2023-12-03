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
	public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
	{
		private readonly LeaveManagementDbContext context;
		public LeaveRequestRepository(LeaveManagementDbContext context) 
			: base(context)
		{
			this.context = context;
		}

		public async Task ChangeApprovalStatus(LeaveRequest leaveRequest, bool? approvalStatus)
		{
			leaveRequest.Approved = approvalStatus.Value;
			context.Entry(leaveRequest).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
		}

		public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetailsAsync()
		{
			var leaveRequests = await context.LeaveRequests
				.Include(x => x.LeaveType)
				.ToListAsync();

			return leaveRequests;
		}

        public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetailsAsync(string userId)
        {
            var leaveRequests = await context.LeaveRequests.Where(x => x.RequestingEmployeeId== userId)
				.Include(x => x.LeaveType)
				.ToListAsync();

			return leaveRequests;
        }

        public async Task<LeaveRequest> GetLeaveRequestWithDetailsAsync(int id)
		{
			var leaveRequest = await context.LeaveRequests
				.Include(x => x.LeaveType)
				.FirstOrDefaultAsync(x => x.Id== id);

			return leaveRequest; 
		}
	}
}
