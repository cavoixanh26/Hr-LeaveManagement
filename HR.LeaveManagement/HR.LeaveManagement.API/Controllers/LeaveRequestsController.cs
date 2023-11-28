using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HR.LeaveManagement.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LeaveRequestsController : ControllerBase
	{
		private readonly IMediator mediator;

		public LeaveRequestsController(IMediator mediator)
		{
			this.mediator = mediator;
		}


		// GET: api/<LeaveRequestController>
		[HttpGet]
		public async Task<ActionResult<List<LeaveRequestDto>>> Get()
		{
			var leaveRequests = await mediator.Send(new GetLeaveRequestListRequest());
			return Ok(leaveRequests);
		}

		// GET api/<LeaveRequestController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult> Get(int id)
		{
			var leaveRequest = await mediator.Send(new GetLeaveRequestDetailRequest { Id = id });
			return Ok(leaveRequest);
		}

		// POST api/<LeaveRequestController>
		[HttpPost]
		public async Task<ActionResult> Post([FromBody] CreateLeaveRequestDto request)
		{
			var command = new CreateLeaveRequestCommand { LeaveRequestDto = request };
			var repsonse = await mediator.Send(command);
			return Ok(repsonse);
		}

		// PUT api/<LeaveRequestController>/5
		[HttpPut("{id}")]
		public async Task<ActionResult> Put(int id, [FromBody] UpdateLeaveRequestDto request)
		{
			var command = new UpdateLeaveRequestCommand {Id= id, LeaveRequestDto = request };
			await mediator.Send(command);
			return NoContent();
		}

		[HttpPut("changeapproval/{id}")]
		public async Task<ActionResult> ChangeApproval(int id,
			[FromBody] ChangeLeaveRequestApprovalDto changeLeaveRequestApproval)
		{
			var command = new UpdateLeaveRequestCommand 
			{ 
				Id = id, 
				ChangeLeaveRequestApprovalDto = changeLeaveRequestApproval 
			};
			await mediator.Send(command);
			return NoContent();
		}


		// DELETE api/<LeaveRequestController>/5
		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var command = new DeleteLeaveRequestCommand { Id = id };
			await mediator.Send(command);
			return NoContent();
		}
	}
}
