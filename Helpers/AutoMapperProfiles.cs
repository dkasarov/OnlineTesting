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
            CreateMap<TestForAddingDto, Test>();
            CreateMap<QuestionForAddingDto, TestQuestion>();
            CreateMap<AnswerForAddingDto, TestQuestionAnswer>();
            CreateMap<Test, TestDetailsForUserDto>()
                .ForMember(dest => dest.CategoryName, opt =>
                {
                    opt.MapFrom(c => c.Category.Name);
                });
            CreateMap<GeolocationDto, Student>()
                .ForMember(dest => dest.NetworkIP, opt =>
                {
                    opt.MapFrom(s => s.IP); //IP се взима от ipify.org
                })
                .ForMember(dest => dest.Country, opt =>
                {
                    opt.MapFrom(s => s.Location.Country);
                })
                .ForMember(dest => dest.City, opt =>
                {
                    opt.MapFrom(s => s.Location.City);
                })
                .ForMember(dest => dest.Region, opt =>
                {
                    opt.MapFrom(s => s.Location.Region);
                })
                .ForMember(dest => dest.PostalCode, opt =>
                {
                    opt.MapFrom(s => s.Location.PostalCode);
                })
                .ForMember(dest => dest.Lat, opt =>
                {
                    opt.MapFrom(s => s.Location.Lat);
                })
                .ForMember(dest => dest.Lng, opt =>
                {
                    opt.MapFrom(s => s.Location.Lng);
                });
            CreateMap<StudentToTestAddingDto, StudentToTest>();
        }
    }
}
