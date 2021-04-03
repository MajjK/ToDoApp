using System;
using AutoMapper;
using ToDoApp.ViewModel.Tasks;
using ToDoApp.ViewModel.Users;
using ToDoApp.ViewModel.Auth;
using ToDoApp.DB.Model;

namespace ToDoApp.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskViewModel, DbTask>().ReverseMap();
            CreateMap<UserViewModel, DbUser>().ReverseMap();
            CreateMap<RegisterViewModel, DbUser>().ReverseMap();
            CreateMap<PasswordViewModel, DbUser>().ReverseMap();
        }
    }

}
