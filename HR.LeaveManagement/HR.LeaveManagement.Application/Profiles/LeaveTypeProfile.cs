using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Profiles
{
    public class LeaveTypeProfile : Profile
	{
		public LeaveTypeProfile()
		{
			CreateMap<LeaveType, LeaveTypeDto>().ReverseMap();
			CreateMap<LeaveType, CreateLeaveTypeDto>().ReverseMap();
		}
	}
}
