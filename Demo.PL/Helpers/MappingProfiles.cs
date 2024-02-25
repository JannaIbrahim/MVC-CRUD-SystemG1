using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace Demo.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<EmployeeViewModel, Employee>().ReverseMap();

            CreateMap<DepartmentViewModel, Department>().ReverseMap();

            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();

            CreateMap<RoleViewModel, IdentityRole>()
                     .ForMember(d => d.Name, o => o.MapFrom(s => s.RoleName))
                     .ReverseMap();


        }
    }
}
