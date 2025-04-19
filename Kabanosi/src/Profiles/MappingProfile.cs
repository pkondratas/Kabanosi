using AutoMapper;
using Kabanosi.Dtos.Auth;
using Kabanosi.Dtos.Assignment;
using Kabanosi.Dtos.Project;
using Kabanosi.Entities;

namespace Kabanosi.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Auth
        CreateMap<RegisterRequestDto, User>();
        CreateMap<User, LoginResponseDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore());
        
        // Project
        CreateMap<ProjectRequestDto, Project>();
        CreateMap<Project, ProjectResponseDto>();

        // Assignment
        CreateMap<AssignmentRequestDto, Assignment>();
        CreateMap<Assignment, AssignmentResponseDto>();
    }
}