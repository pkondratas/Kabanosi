using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Kabanosi.Constants;
using Kabanosi.Dtos.Invitation;
using Kabanosi.Entities;
using Kabanosi.Exceptions;
using Kabanosi.Repositories;
using Kabanosi.Repositories.UnitOfWork;
using Kabanosi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Kabanosi.Services;

public class InvitationService : IInvitationService
{
    private readonly InvitationRepository _invitationRepo;
    private readonly ProjectMemberRepository _projectMemberRepo;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InvitationService(InvitationRepository invitationRepo, ProjectMemberRepository projectMemberRepo,
        UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _invitationRepo = invitationRepo;
        _projectMemberRepo = projectMemberRepo;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<InvitationResponseDto> CreateInvitationAsync(Guid projectId, CreateInvitationDto invitationDto,
        CancellationToken cancellationToken)
    {
        var days = invitationDto.ValidDays ?? 7;
        if (days is < 1 or > 31)
            throw new ValidationException("ValidDays must be between 1 and 31.");
        
        var targetUser = await _userManager.FindByEmailAsync(invitationDto.TargetEmail)
                         ?? throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);

        var alreadyMember = await _projectMemberRepo.ExistsAsync(
            pm => pm.ProjectId == projectId && pm.UserId == targetUser.Id, cancellationToken);
        if (alreadyMember)
            throw new ConflictException(ErrorMessages.USER_ALREADY_PROJECT_MEMBER);
        
        // TODO - check if invitation already exist and is still pending:
        // if accepted, declined, cancelled or expired - can send a  new one, else - no
        
        var invite = new Invitation
        {
            ValidUntil = DateTime.UtcNow.AddDays(days),
            ProjectRole = invitationDto.TargetRole!.Value,
            InvitationStatus = InvitationStatus.Pending,
            ProjectId = projectId,
            UserId = targetUser.Id,
        };
        
        await _invitationRepo.InsertAsync(invite, cancellationToken);
        await _unitOfWork.SaveAsync();
        
        // TODO - later add notification service to send live invite to intended user
        // await _notificationService.SendInviteAsync(?);
        
        return _mapper.Map<InvitationResponseDto>(invite);
    }
}