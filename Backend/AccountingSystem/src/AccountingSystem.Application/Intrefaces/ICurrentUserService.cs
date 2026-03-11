namespace AccountingSystem.Application.Intrefaces
{
    public interface ICurrentUserService
    {
        int GetFirmId();
        int GetUserId();
        string GetUserEmail();
        string GetUserRole();
    }
}