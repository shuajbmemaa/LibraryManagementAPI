using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace LMS.Application.Wrappers
{
    public static class PagedQueryExtension
    {
        public static async Task<PagedResponse<T>> ToPagedAsync<T>(this IQueryable<T> query, ICurrentPaging paging, bool ignorePagination = false)
        {
            if (ignorePagination)
            {
                return new PagedResponse<T>((await query.ToListAsync()).ToPagedList());
            }

            return new PagedResponse<T>(await query.ToPagedListAsync(paging.PageNumber, paging.PageSize));
        }

        public static async Task<PagedResponse<T>> ToPagedAsync<T>(this ICollection<T> query, ICurrentPaging paging, bool ignorePagination = false)
        {
            if (ignorePagination)
            {
                List<T> response = query.ToList();
                return new PagedResponse<T>(response.ToPagedList());
            }

            return new PagedResponse<T>(await query.ToPagedListAsync(paging.PageNumber, paging.PageSize));
        }
    }
}