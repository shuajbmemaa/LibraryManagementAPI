namespace LMS.Application.Services.User
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        bool IsAdmin { get; }
    }
}
