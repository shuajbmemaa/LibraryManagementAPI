using LMS.Application.DTO.Request.Book;
using LMS.Application.DTO.Response.Book;
using LMS.Application.Wrappers;

namespace LMS.Application.Services.Book
{
    public interface IBookService
    {
        Task<BaseResponse<List<BookResponseDto>>> GetAllAsync(BookFilterDto? filter = null);
        Task<BaseResponse<BookResponseDto?>> GetByIdAsync(Guid id);
        Task<BaseResponse<BookResponseDto>> CreateAsync(CreateBookDto dto);
        Task<BaseResponse<BookResponseDto?>> UpdateAsync(Guid id, UpdateBookDto dto);
        Task<BaseResponse<bool>> DeleteAsync(Guid id);
    }
}
