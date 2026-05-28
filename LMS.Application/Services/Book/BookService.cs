using LMS.Application.DTO.Request.Book;
using LMS.Application.DTO.Response.Account;
using LMS.Application.DTO.Response.Book;
using LMS.Application.Services.User;
using LMS.Application.Wrappers;
using LMS.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMS.Application.Services.Book
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICurrentUser _currentUser;

        public BookService(IBookRepository bookRepository, ICurrentUser currentUser)
        {
            _bookRepository = bookRepository;
            _currentUser = currentUser;
        }

        public async Task<BaseResponse<BookResponseDto>> CreateAsync(CreateBookDto dto)
        {
            var book = MapToBook(dto);

            await _bookRepository.Create(book);

            var result = MapToDto(book);

            return BaseResponse<BookResponseDto>.Ok(result);
        }

        private BookResponseDto MapToDto(Domain.Entities.Book book)
        {
            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                ReadingStatus = book.ReadingStatus,
                User = new UserInfoDto
                {
                    Id = book.User?.Id ?? Guid.Empty,
                    UserName = book.User?.Name ?? string.Empty
                }
            };
        }

        private Domain.Entities.Book MapToBook(CreateBookDto dto)
        {
            var userId = _currentUser.UserId;

            return new Domain.Entities.Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Genre = dto.Genre,
                UserId = userId,
                CreatedBy = userId.ToString()
            };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            var book = await _bookRepository.GetById(id);

            if (book == null)
            {
                return BaseResponse<bool>.NotFound("Book not found");
            }

            if (!_currentUser.IsAdmin && book.UserId != _currentUser.UserId)
                return BaseResponse<bool>.BadRequest("You are not allowed to delete this book");

            var deleted = await _bookRepository.Delete(id) > 0;

            return BaseResponse<bool>.Ok(deleted);
        }

        public static Expression<Func<Domain.Entities.Book, BookResponseDto>> BookMapper =>
            b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre,
                ReadingStatus = b.ReadingStatus,
                User = new UserInfoDto
                {
                    Id = b.UserId,
                    UserName = b.User.Name
                }
            };

        public async Task<BaseResponse<PagedList<BookResponseDto>>> GetAllAsync(BookFilterDto? filter = null)
        {
            var response = _bookRepository
                .GetAll()
                .Where(b => _currentUser.IsAdmin || b.UserId == _currentUser.UserId)
                .ApplyFilters(filter)
                .OrderBy(b => b.Id)
                .Select(BookMapper);

            var pagedResponse = await PagedList<BookResponseDto>.ToPagedListAsync(response);

            return BaseResponse<PagedList<BookResponseDto>>.Ok(pagedResponse);
        }

        public async Task<BaseResponse<BookResponseDto?>> GetByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetAll()
                .Include(b => b.User)
                .Where(x => _currentUser.IsAdmin || x.UserId == _currentUser.UserId)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
            {
                return BaseResponse<BookResponseDto?>.NotFound("Book not found or access denied");
            }

            var response = MapToDto(book);

            return BaseResponse<BookResponseDto?>.Ok(response);
        }

        public async Task<BaseResponse<BookResponseDto?>> UpdateAsync(Guid id, UpdateBookDto dto)
        {
            var book = await _bookRepository.GetById(id);

            if (book == null)
                return BaseResponse<BookResponseDto?>.NotFound("Book not found");

            if (!_currentUser.IsAdmin && book.UserId != _currentUser.UserId)
                return BaseResponse<BookResponseDto?>.BadRequest("You are not allowed to update this book");

            MapUpdate(book, dto);

            await _bookRepository.Update(book);

            return BaseResponse<BookResponseDto?>.Ok(MapToDto(book));
        }

        private void MapUpdate(Domain.Entities.Book book, UpdateBookDto dto)
        {
            book.Title = dto.Title ?? book.Title;
            book.Author = dto.Author ?? book.Author;
            book.Genre = dto.Genre ?? book.Genre;
            book.ReadingStatus = dto.ReadingStatus ?? book.ReadingStatus;
        }
    }
}