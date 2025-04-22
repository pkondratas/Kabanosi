namespace Kabanosi.Constants;

public enum InvitationStatus
{
    Pending,      // waiting for user
    Accepted,
    Declined,
    Cancelled     // sender revoked before decision
}