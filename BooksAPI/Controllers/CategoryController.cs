using BooksAPI.DTOs.CategoryDtos;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BooksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await _categoryService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("withbooks")]
    public async Task<IActionResult> GetWithBooks()
    {
        var categories = await _categoryService.GetCategoriesWithBooksAsync();
        var json = JsonConvert.SerializeObject(categories, Formatting.Indented,
                       new JsonSerializerSettings
                       {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        return Ok(json);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Post(AddCategoryDto dto)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.AddCategoryAsync(dto);
            return Ok();
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Put(CategoryDto category)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.UpdateCategoryAsync(category);
            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        await _categoryService.DeleteCategoryAsync(Id);
        return Ok();
    }
    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(int pageSize = 10, int pageNumber = 1)
    {
        var paged = await _categoryService.GetPagetCategories(pageSize, pageNumber);

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
    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] FilterParametrs parametrs)
    {
        var books = await _categoryService.Filter(parametrs);
        return Ok(books);
    }

}
