using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HR.LeaveManagement.MVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveTypesController : Controller
	{
		private readonly ILeaveTypeService leaveTypeService;
        private readonly ILeaveAllocationService leaveAllocationService;

        public LeaveTypesController(
			ILeaveTypeService leaveTypeService,
			ILeaveAllocationService leaveAllocationService)
		{
			this.leaveTypeService = leaveTypeService;
            this.leaveAllocationService = leaveAllocationService;
        }


		// GET: LeaveTypesController
		public async Task<ActionResult> Index()
		{
			var model = await leaveTypeService.GetLeaveTypes();
			return View(model);
		}

		// GET: LeaveTypesController/Details/5
		public async Task<ActionResult> Details(int id)
		{
            var model = await leaveTypeService.GetLeaveTypeDetails(id);
            return View(model);
        }

		// GET: LeaveTypesController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: LeaveTypesController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(CreateLeaveTypeVM request)
		{
			try
			{
				var response = await leaveTypeService.CreateLeaveType(request);
				if (response.Success)
				{
					return RedirectToAction(nameof(Index));
				}
				ModelState.AddModelError("", response.ValidationErrors);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
				return View(request);
		}

		// GET: LeaveTypesController/Edit/5
		public async Task<ActionResult> Edit(int id)
		{
			var model = await leaveTypeService.GetLeaveTypeDetails(id);
			return View(model);
		}

		// POST: LeaveTypesController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int id, LeaveTypeVM request)
		{
			try
			{
				var response = await leaveTypeService.UpdateLeaveType(id, request);
				if (response.Success)
					return RedirectToAction(nameof(Index));
				
				ModelState.AddModelError("", response.ValidationErrors);
			}
			catch(Exception ex)
			{
                ModelState.AddModelError("", ex.Message);
            }
			return View(request);	
        }


		// POST: LeaveTypesController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(int id)
		{
            try
            {
                var response = await leaveTypeService.DeleteLeaveType(id);
                if (response.Success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", response.ValidationErrors);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Allocate(int id)
		{
			try
			{
                var response = await leaveAllocationService.CreateLeaveAllocations(id);
                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest();
        }
    }
}
