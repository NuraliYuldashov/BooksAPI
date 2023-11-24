using AutoMapper;
using BooksAPI.DTOs.CategoryDtos;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer.Service;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWorkInterface _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWorkInterface unitOfWork,
                           IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task AddCategoryAsync(AddCategoryDto newCategory)
    {
        var category = _mapper.Map<Category>(newCategory);
        await _unitOfWork.CategoryInterface.AddAsync(category);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        _unitOfWork.CategoryInterface.Delete(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<PagedList<CategoryDto>> Filter(FilterParametrs parametrs)
    {
        var list = await _unitOfWork.CategoryInterface.GetAllAsync();

        // Filter by name
        if (parametrs.Title is not "")
        {
            list = list.Where(book => book.Name.ToLower()
                       .Contains(parametrs.Title.ToLower()));
        }
        var dtos = list.Select(book => _mapper.Map<CategoryDto>(book)).ToList();

        // Order by title
        if (parametrs.orderByTitle)
        {
            dtos = dtos.OrderBy(book => book.Name).ToList();
        }
        else
        {
            dtos = dtos.OrderByDescending(book => book.Name).ToList();
        }

        PagedList<CategoryDto> pagedList = new(dtos, dtos.Count,
                                                          parametrs.PageNumber, parametrs.pageSize);

        return pagedList.ToPagedList(dtos, parametrs.PageSize, parametrs.PageNumber);
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var list = await _unitOfWork.CategoryInterface.GetAllAsync();
        return list.Select(c => _mapper.Map<CategoryDto>(c)).ToList();
    }

    public async Task<List<CategoryDto>> GetCategoriesWithBooksAsync()
    {
        var list = await _unitOfWork.CategoryInterface.GetAllCategoriesWithBoksAsyc();
        return list.Select(c => _mapper.Map<CategoryDto>(c)).ToList();
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.CategoryInterface.GetByIdAsync(id);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<PagedList<CategoryDto>> GetPagetCategories(int pageSize, int pageNumber)
    {
        var list = await _unitOfWork.CategoryInterface.GetAllAsync();
        var dtos = list.Select(c => _mapper.Map<CategoryDto>(c))
                       .ToList();
        PagedList<CategoryDto> pagedList = new(dtos,
                                                                 dtos.Count(),
                                                                 pageNumber,
                                                                 pageSize);
        return pagedList.ToPagedList(dtos, pageSize, pageNumber);
    }

    public async Task UpdateCategoryAsync(CategoryDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        _unitOfWork.CategoryInterface.Update(category);
        await _unitOfWork.SaveAsync();
    }
}
