namespace Ghumfir.Application.Contracts;

public interface IUserAccessor
{
    public string GetMobile();
    public string GetApprovalStatus();
    public string GetActiveStatus();
    public string GetRole();
    public string GetFullname();
    public string GetUserId();
    public string GetForceChangePassword();
}