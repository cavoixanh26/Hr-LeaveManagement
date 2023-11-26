using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Profiles
{
    public class LeaveAllocationProfile : Profile
	{
		public LeaveAllocationProfile()
		{
			CreateMap<LeaveAllocation, LeaveAllocationDto>().ReverseMap();	
		}
	}
}
