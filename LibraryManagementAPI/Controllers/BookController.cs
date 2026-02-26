using LMS.Application.DTO.Request.Book;
using LMS.Application.DTO.Response.Book;
using LMS.Application.Services.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Authorize]
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("getAllBooks")]
        public async Task<IActionResult> GetAll([FromQuery] BookFilterDto? dto)
        {
            var result = await _bookService.GetAllAsync(dto);
            return CreateResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookDto dto)
        {
            var result = await _bookService.CreateAsync(dto);
            return CreateResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _bookService.DeleteAsync(id);
            return CreateResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _bookService.GetByIdAsync(id);
            return CreateResponse(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateBookDto dto)
        {
            var result = await _bookService.UpdateAsync(id, dto);
            return CreateResponse(result);
        }
    }
}
