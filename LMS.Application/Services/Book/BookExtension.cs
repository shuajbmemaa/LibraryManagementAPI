using LMS.Application.DTO.Response.Book;

namespace LMS.Application.Services.Book
{
    public static class BookExtension
    {
        public static IQueryable<Domain.Entities.Book> ApplyFilters(this IQueryable<Domain.Entities.Book> query, BookFilterDto? filter)
        {
            if (filter == null) return query;

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(b => b.Title.Contains(filter.Title));

            if (!string.IsNullOrWhiteSpace(filter.Author))
                query = query.Where(b => b.Author.Contains(filter.Author));

            if (!string.IsNullOrWhiteSpace(filter.Genre))
                query = query.Where(b => b.Genre.Contains(filter.Genre));

            if (filter.ReadingStatus != null)
                query = query.Where(b => b.ReadingStatus == filter.ReadingStatus);

            return query;
        }
    }
}
