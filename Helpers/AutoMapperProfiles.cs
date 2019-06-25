using AutoMapper;
using OnlineTesting.Dtos;
using OnlineTesting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserForDetailsDto>();
            CreateMap<User, UserForListDto>();
            CreateMap<CategoryForCreationDto, Category>();
            CreateMap<UserForUpdateDto, User>();
        }
    }
}
