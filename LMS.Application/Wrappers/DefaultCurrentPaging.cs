namespace LMS.Application.Wrappers
{
    public class DefaultCurrentPaging : ICurrentPaging
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}