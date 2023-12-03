using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
	public class LeaveRequestService : BaseHttpService, ILeaveRequestService
	{
        private readonly IMapper mapper;

        public LeaveRequestService(
			IClient client, 
			ILocalStorageService localStorage,
			IMapper mapper) 
			: base(client, localStorage)
        {
            this.mapper = mapper;
        }

        public async Task ApproveLeaveRequest(int id, bool approved)
		{
			AddBearerToken();
			try
			{
				var request = new ChangeLeaveRequestApprovalDto
				{
					Id = id,
					Approved = approved
				};
				await client.ChangeapprovalAsync(id, request);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM leaveRequest)
		{
			try
			{
                var response = new Response<int>();
                AddBearerToken();
                var createLeaveRequestDto = mapper.Map<CreateLeaveRequestDto>(leaveRequest);
                var apiResponse = await client.LeaveRequestsPOSTAsync(createLeaveRequestDto);
                if (apiResponse.Success)
                {
                    response.Data = apiResponse.Id;
                    response.Success = true;
                }
                else
                {
                    foreach (var error in apiResponse.Errors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }
                return response;
            }
			catch (ApiException ex)
			{
				return ConvertApiExceptions<int>(ex);
			}
		}

		public Task DeleteLeaveRequest(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList()
		{
			AddBearerToken();
			var leaveRequest = await client.LeaveRequestsAllAsync(isLogggedInUser: false);

			var model = new AdminLeaveRequestViewVM
			{
				TotalRequests = leaveRequest.Count,
				ApprovedRequests = leaveRequest.Count(x => x.Approved == true),
				PendingRequests = leaveRequest.Count(x => x.Approved is null),
				RejectedRequests = leaveRequest.Count(x => x.Approved == false),
				LeaveRequests = mapper.Map<List<LeaveRequestVM>>(leaveRequest)
			};

			return model;
		}

		public async Task<LeaveRequestVM> GetLeaveRequest(int id)
		{
			AddBearerToken();
			var leaveRequest = await client.LeaveRequestsGETAsync(id);
			var leaveRequestVM = mapper.Map<LeaveRequestVM>(leaveRequest);

			return leaveRequestVM;
        }

		public Task<EmployeeLeaveRequestViewVM> GetUserLeaveRequests()
		{
			throw new NotImplementedException();
		}
	}
}
