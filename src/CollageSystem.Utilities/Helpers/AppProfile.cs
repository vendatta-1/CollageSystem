
using AutoMapper;
using CollageSystem.Core.Models;

namespace CollageSystem.Utilities.Helpers
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {

            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<Department, DepartmentBaseDto>();
            CreateMap<DepartmentBaseDto, Department>();
            CreateMap<Department, DepartmentResponseDto>();
            CreateMap<DepartmentBaseDto, DepartmentResponseDto>();
            CreateMap<DepartmentUpdateDto, Department>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null));

            CreateMap<StudentCreateDto, Student>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string
                    .Concat(src.FirstName, src.LastName ?? "")));
            CreateMap<StudentBaseDto, Student>()
                .ForMember(x => x.Name, opt => opt.MapFrom(
                    src => string.Concat(src.FirstName, src.LastName ?? "")));
            CreateMap<Student, StudentResponseDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src =>
                    src.Name.Split().FirstOrDefault() ?? string.Empty))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src =>
                    src.Name.Split().Skip(1).FirstOrDefault() ?? string.Empty));

            CreateMap<StudentUpdateDto, Student>();


            CreateMap<Course, CourseBaseDto>()
                .ForMember(x => x.EndDate, opt => opt.MapFrom(x => DateOnly.FromDateTime(x.EndDate)))
                .ForMember(x => x.StartDate, opt => opt.MapFrom(x => DateOnly.FromDateTime(x.StartDate)));

            CreateMap<CourseBaseDto, Course>();
        }
    }
}