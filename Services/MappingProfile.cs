using System;
using AutoMapper;
using ToDoApp.ViewModel.Tasks;
using ToDoApp.ViewModel.Users;
using ToDoApp.DB.Model;

namespace ToDoApp.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DbTask, TaskViewModel>();
            CreateMap<TaskViewModel, DbTask>();
            CreateMap<DbUser, UserViewModel>();
            CreateMap<UserViewModel, DbUser>();
        }
    }

}
