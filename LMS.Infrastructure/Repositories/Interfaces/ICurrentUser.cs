namespace LMS.Infrastructure.Repositories.Interfaces
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        bool IsAdmin { get; }
    }
}
