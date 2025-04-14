using AutoMapper;
using Kabanosi.Dtos.Project;
using Kabanosi.Entities;

namespace Kabanosi.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Project
        CreateMap<ProjectRequestDto, Project>();
        CreateMap<Project, ProjectResponseDto>();
    }
}