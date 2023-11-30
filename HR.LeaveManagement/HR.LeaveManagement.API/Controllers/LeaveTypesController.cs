﻿using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using HR.LeaveManagement.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HR.LeaveManagement.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class LeaveTypesController : ControllerBase
	{

		private readonly IMediator mediator;

		public LeaveTypesController(IMediator mediator)
		{
			this.mediator = mediator;
		}


		// GET: api/<LeaveTypesController>
		[HttpGet]
		public async Task<ActionResult<List<LeaveTypeDto>>> Get()
		{
			var leaveTypes = await mediator.Send(new GetLeaveTypeListRequest());
			return Ok(leaveTypes);
		}

		// GET api/<LeaveTypesController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<LeaveTypeDto>> Get(int id)
		{
			var leaveType = await mediator.Send(new GetLeaveTypeDetailRequest { Id = id});
			return Ok(leaveType);
		}

		// POST api/<LeaveTypesController>
		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateLeaveTypeDto request)
		{
			var command = new CreateLeaveTypeCommand { LeaveTypeDto = request };
			var response = await mediator.Send(command);

			return Ok(response);
		}

		// PUT api/<LeaveTypesController>/5
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> Put([FromBody] LeaveTypeDto leaveType)
		{
			var command = new UpdateLeaveTypeCommand { LeaveTypeDto = leaveType };
			await mediator.Send(command);	
			return NoContent();
		}

		// DELETE api/<LeaveTypesController>/5
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> Delete(int id)
		{
			var command = new DeleteLeaveTypeCommand { Id = id };
			await mediator.Send(command);	
			return NoContent();
		}
	}
}
