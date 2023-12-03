using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Profiles
{
    public class LeaveRequestProfile : Profile
	{
		public LeaveRequestProfile()
		{
			CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();
			CreateMap<LeaveRequest, LeaveRequestListDto>()
				.ForMember(dest => dest.DateRequest, opt => opt.MapFrom(src => src.DateCreated))
				.ReverseMap();
			CreateMap<LeaveRequest, CreateLeaveRequestDto>().ReverseMap();
		}
	}
}
