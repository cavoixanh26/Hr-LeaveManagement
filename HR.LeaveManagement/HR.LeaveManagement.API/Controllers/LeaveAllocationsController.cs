using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HR.LeaveManagement.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LeaveAllocationsController : ControllerBase
	{
		private readonly IMediator mediator;

		public LeaveAllocationsController(IMediator mediator)
		{
			this.mediator = mediator;
		}


		// GET: api/<LeaveAllocationController>
		[HttpGet]
		public async Task<ActionResult<List<LeaveAllocationDto>>> Get(bool isLoggedInUser = false)
		{
			var leaveAllocations = await mediator.Send(new GetLeaveAllocationListRequest
			{
				IsLoggedInUser = isLoggedInUser
			});

			return Ok(leaveAllocations);
		}

		// GET api/<LeaveAllocationController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<LeaveAllocationDto>> Get(int id)
		{
			var leaveAllocation = await mediator.Send(new GetLeaveAllocationDetailRequest { Id = id});	
			return Ok(leaveAllocation);
		}

		// POST api/<LeaveAllocationController>
		[HttpPost]
		public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateLeaveAllocationDto request)
		{
			var command = new CreateLeaveAllocationCommand { LeaveAllocationDto = request };
			var response = await mediator.Send(command);

			return Ok(response);
		}

		// PUT api/<LeaveAllocationController>/5
		[HttpPut("{id}")]
		public async Task<ActionResult> Put([FromBody] UpdateLeaveAllocationDto leaveAllocation)
		{
			var command = new UpdateLeaveAllocationCommand { LeaveAllocationDto = leaveAllocation };
			await mediator.Send(command);
			return NoContent();
		}

		// DELETE api/<LeaveAllocationController>/5
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteAsync(int id)
		{
			var command = new DeleteLeaveAllocationCommand { Id = id };
			await mediator.Send(command);
			return NoContent();
		}
	}
}
