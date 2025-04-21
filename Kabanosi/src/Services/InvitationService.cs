using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public InvitationService(InvitationRepository invitationRepo, ProjectMemberRepository projectMemberRepo,
        UserManager<User> userManager, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _invitationRepo = invitationRepo;
        _projectMemberRepo = projectMemberRepo;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<IList<UserInvitesResponseDto>> GetUserInvitesAsync(
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new UnauthorizedAccessException();

        var userValidPendingInvites = await _invitationRepo.GetAllAsync(
            pageSize, pageNumber, cancellationToken,
            i => i.UserId == userId &&
                 i.InvitationStatus == InvitationStatus.Pending &&
                 i.ValidUntil >= DateTime.UtcNow,
            includes: i => i.Project);

        return _mapper.Map<IList<UserInvitesResponseDto>>(userValidPendingInvites);
    }

    public async Task<IList<InvitationResponseDto>> GetProjectInvitesAsync(Guid projectId, int pageSize, int pageNumber,
        CancellationToken cancellationToken)
    {
        var allInvites = await _invitationRepo.GetAllAsync(
            pageSize, pageNumber, cancellationToken,
            i => i.ProjectId == projectId,
            includes: i => i.User);

        return _mapper.Map<IList<InvitationResponseDto>>(allInvites);
    }

    public async Task<InvitationResponseDto> CreateInviteAsync(Guid projectId, CreateInvitationDto invitationDto,
        CancellationToken cancellationToken)
    {
        var days = invitationDto.ValidDays ?? 7;
        if (days is < 1 or > 31)
            throw new ValidationException("Valid Days must be between 1 and 31.");

        var targetUser = await _userManager.FindByEmailAsync(invitationDto.TargetEmail)
                         ?? throw new NotFoundException($"User {invitationDto.TargetEmail} was not found.");

        var alreadyMember = await _projectMemberRepo.ExistsAsync(
            pm => pm.ProjectId == projectId && pm.UserId == targetUser.Id, cancellationToken);
        if (alreadyMember)
            throw new ConflictException($"User {targetUser.Email} is already a project member");

        var existingInvite = await _invitationRepo.FirstOrDefaultTrackedAsync(
            i => i.ProjectId == projectId &&
                 i.UserId == targetUser.Id &&
                 i.InvitationStatus == InvitationStatus.Pending,
            cancellationToken);

        if (existingInvite is not null)
        {
            if (existingInvite.ValidUntil < DateTime.UtcNow)
            {
                await _invitationRepo.DeleteAsync(existingInvite, cancellationToken);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new ConflictException("An active invitation for this user already exists.");
            }
        }

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

        // TODO - later add notificationService to send live invite to intended user here
        // await _notificationService.SendInviteAsync(?);

        return _mapper.Map<InvitationResponseDto>(invite);
    }

    public async Task AcceptInviteAsync(Guid invitationId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new UnauthorizedAccessException();

        var invite = await _invitationRepo.FirstOrDefaultTrackedAsync(
            i => i.Id == invitationId,
            cancellationToken);

        if (invite is null)
            throw new NotFoundException("Invitation not found.");

        if (invite.UserId != userId)
            throw new UnauthorizedAccessException("You are not the recipient of this invite.");

        await CheckAndDeleteIfExpired(invite, cancellationToken);

        if (invite.InvitationStatus != InvitationStatus.Pending)
            throw new ConflictException("This invitation is no longer valid.");

        var newProjectMember = new ProjectMember
        {
            ProjectId = invite.ProjectId,
            UserId = userId,
            ProjectRole = invite.ProjectRole
        };

        await _projectMemberRepo.InsertAsync(newProjectMember, cancellationToken);
        invite.InvitationStatus = InvitationStatus.Accepted;

        await _unitOfWork.SaveAsync();
    }

    public async Task DeclineInviteAsync(Guid invitationId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new UnauthorizedAccessException();

        var invite = await _invitationRepo.FirstOrDefaultTrackedAsync(i => i.Id == invitationId, cancellationToken);
        if (invite is null)
            throw new NotFoundException("Invitation not found.");

        if (invite.UserId != userId)
            throw new UnauthorizedAccessException("You are not the recipient of this invite.");

        await CheckAndDeleteIfExpired(invite, cancellationToken);

        if (invite.InvitationStatus != InvitationStatus.Pending)
            throw new ConflictException("This invitation is no longer valid.");

        invite.InvitationStatus = InvitationStatus.Declined;
        await _unitOfWork.SaveAsync();
    }

    public async Task CancelInvitationAsync(Guid invitationId, CancellationToken cancellationToken)
    {
        var invite = await _invitationRepo.FirstOrDefaultTrackedAsync(i => i.Id == invitationId, cancellationToken);

        if (invite is null)
            throw new NotFoundException("Invitation not found.");

        await CheckAndDeleteIfExpired(invite, cancellationToken);

        if (invite.InvitationStatus != InvitationStatus.Pending)
            throw new ConflictException("This invitation is no longer valid.");

        invite.InvitationStatus = InvitationStatus.Cancelled;
        await _unitOfWork.SaveAsync();
    }

    private async Task CheckAndDeleteIfExpired(Invitation invite, CancellationToken cancellationToken)
    {
        if (invite.ValidUntil < DateTime.UtcNow)
        {
            await _invitationRepo.DeleteAsync(invite, cancellationToken);
            await _unitOfWork.SaveAsync();
            throw new ConflictException("This invitation has expired.");
        }
    }
}