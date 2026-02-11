using LMS.Domain.Entities;

namespace LMS.Infrastructure.Repositories.Interfaces
{
    public interface IOwnerGuard
    {
        Task EnsureOwnerAsync<TEntity>(Guid entityId)
            where TEntity : BaseEntity;
    }
}
