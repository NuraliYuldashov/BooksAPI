using BooksAPI.DTOs.BookDtos;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BooksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var books = await _bookService.GetBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Post(AddBookDto dto)
    {
        if (ModelState.IsValid)
        {
            await _bookService.AddBookAsync(dto);
            return Ok();
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Put(BookDto book)
    {
        if (ModelState.IsValid)
        {
            await _bookService.UpdateBookAsync(book);
            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        await _bookService.DeleteBookAsync(Id);
        return Ok();
    }
    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] FilterParametrs parametrs)
    {
        var books = await _bookService.Filter(parametrs);
        return Ok(books);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(int pageSize = 10, int pageNumber = 1)
    {
        var paged = await _bookService.GetPagetBooks(pageSize, pageNumber);

        var metaData = new
        {
            paged.TotalCount,
            paged.PageSize,
            paged.CurrentPage,
            paged.HasNext,
            paged.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metaData));

        return Ok(paged.Data);
    }
}
