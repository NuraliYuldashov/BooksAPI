using BooksAPI.DTOs.BookDtos;
using BooksAPI.DTOs.CategoryDtos;
using BusinessLogicLayer.Helpers;

namespace BusinessLogicLayer.Interfaces;

public interface ICategoryService
{
    Task<PagedList<CategoryDto>> Filter(FilterParametrs parametrs);
    Task<PagedList<CategoryDto>> GetPagetCategories(int pageSize, int pageNumber);
    Task<List<CategoryDto>> GetCategoriesAsync();
    Task<List<CategoryDto>> GetCategoriesWithBooksAsync();
    Task<CategoryDto> GetCategoryByIdAsync(int id);
    Task AddCategoryAsync(AddCategoryDto newCategory);
    Task UpdateCategoryAsync(CategoryDto categoryDto);
    Task DeleteCategoryAsync(int id);
}
