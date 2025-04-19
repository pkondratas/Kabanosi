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
using Kabanosi.Specifications;
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

        var userPendingInvites = await _invitationRepo.GetAllAsync(pageSize, pageNumber, cancellationToken,
            InvitationSpecifications.ForUserWithStatus(userId, InvitationStatus.Pending), includes: i => i.Project);
        
        return _mapper.Map<IList<UserInvitesResponseDto>>(userPendingInvites);
    }

    public async Task<InvitationResponseDto> CreateInviteAsync(Guid projectId, CreateInvitationDto invitationDto,
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

        var existingInvite = await _invitationRepo
            .FirstOrDefaultAsync(
                i => i.ProjectId == projectId &&
                     i.UserId == targetUser.Id &&
                     i.InvitationStatus == InvitationStatus.Pending &&
                     i.ValidUntil > DateTime.UtcNow,
                cancellationToken);

        if (existingInvite is not null)
            throw new ConflictException("Invitation for this user already exists.");

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
}