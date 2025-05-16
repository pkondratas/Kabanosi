using AutoMapper;
using Kabanosi.Dtos.Auth;
using Kabanosi.Dtos.Assignment;
using Kabanosi.Dtos.AssignmentLabel;
using Kabanosi.Dtos.AssignmentStatus;
using Kabanosi.Dtos.Project;
using Kabanosi.Dtos.Invitation;
using Kabanosi.Dtos.ProjectMember;
using Kabanosi.Entities;
using Microsoft.AspNetCore.JsonPatch;

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
        CreateMap<Project, ProjectRequestDto>();

        // Assignment
        CreateMap<AssignmentRequestDto, Assignment>();
        CreateMap<Assignment, AssignmentResponseDto>()
            .ForMember(dest => dest.AssignmentLabelName, opt => opt.MapFrom(src => src.AssignmentLabel == null ? null : src.AssignmentLabel.Name));

        // Invitation
        CreateMap<Invitation, InvitationResponseDto>()
            .ForMember(dest => dest.InvitationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TargetEmail, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.ProjectRole))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.InvitationStatus));

        CreateMap<Invitation, UserInvitesResponseDto>()
            .ForMember(dest => dest.InvitationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
            .ForMember(dest => dest.RoleOffered, opt => opt.MapFrom(src => src.ProjectRole));

        // AssignmentStatus
        CreateMap<AssignmentStatus, AssignmentStatusResponseDto>();

        // AssignmentLabel
        CreateMap<AssignmentLabel, AssignmentLabelResponseDto>();

        // ProjectMember
        CreateMap<ProjectMemberUpdateRequestDto, ProjectMember>();
        CreateMap<ProjectMember, ProjectMemberResponseDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
    }
}