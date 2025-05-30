﻿using Kabanosi.Dtos.AssignmentStatus;

namespace Kabanosi.Services.Interfaces;

public interface IAssignmentStatusService
{
    Task<AssignmentStatusResponseDto> CreateAssignmentStatusAsync(
        Guid projectId,
        AssignmentStatusRequestDto request,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<AssignmentStatusResponseDto>> CreateDefaultAssignmentStatusesAsync(
        Guid projectId,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<AssignmentStatusResponseDto>> GetAssignmentStatusesAsync(
        Guid projectId, 
        CancellationToken cancellationToken);
    
    Task<IEnumerable<AssignmentStatusResponseDto>> ReorderAssignmentStatusesAsync(
        Guid projectId,
        ReorderAssignmentStatusesRequestDto request,
        CancellationToken cancellationToken);
    
    Task<AssignmentStatusResponseDto> RenameAssignmentStatusAsync(
        Guid id,
        AssignmentStatusRequestDto request,
        CancellationToken cancellationToken);

    Task DeleteAssignmentStatusAsync(Guid id, CancellationToken cancellationToken);

    Task<AssignmentStatusResponseDto> GetInitialAssignmentStatus(
        Guid projectId,
        CancellationToken cancellationToken);
}