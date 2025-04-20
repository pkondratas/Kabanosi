namespace Kabanosi.Constants;

public enum InvitationStatus
{
    Pending,      // waiting for user
    Accepted,
    Declined,
    Expired,      // ValidUntil passed, never answered
    Cancelled     // sender revoked before decision
}