using Microsoft.EntityFrameworkCore;

namespace LMS.Application.Wrappers
{
    public class PagedList<T>
    {
        public List<T> Items { get; private set; }
        public int CurrentPage { get; private set; } = 1;
        public int PageSize { get; private set; } = 10;
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public PagedList(List<T> items, int count)
        {
            Items = items;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
        }

        public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source)
        {
            var count = await source.CountAsync();

            var items = await source
                .Skip(0)
                .Take(10)
                .ToListAsync();

            return new PagedList<T>(items, count);
        }
    }
}