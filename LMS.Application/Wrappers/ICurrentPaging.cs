namespace LMS.Application.Wrappers
{
    public interface ICurrentPaging
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}