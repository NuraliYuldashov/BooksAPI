using AutoMapper;
using BooksAPI.DTOs.BookDtos;
using BooksAPI.DTOs.CategoryDtos;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer.Service;

public class BookService : IBookService
{
    private readonly IUnitOfWorkInterface _unitOfWork;
    private readonly IMapper _mapper;

    public BookService(IUnitOfWorkInterface unitOfWork,
                       IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task AddBookAsync(AddBookDto newBook)
    {
        for (int i = 0; i < 1001; i++)
        {
            var book = _mapper.Map<Book>(newBook);
            book.Category = null;
            book.Title += i;
            book.Description += i;
            book.Price = i;
            await _unitOfWork.BookInterface.AddAsync(book);
            await _unitOfWork.SaveAsync();
        }
    }

    public async Task DeleteBookAsync(int id)
    {
        _unitOfWork.BookInterface.Delete(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<PagedList<BookDto>> Filter(FilterParametrs parametrs)
    {
        var list = await _unitOfWork.BookInterface.GetAllAsync();

        // Filter by title
        if (parametrs.Title is not "")
        {
            list = list.Where(book => book.Title.ToLower()
                       .Contains(parametrs.Title.ToLower()));  
        }

        // Filter by Price
        list = list.Where(book => book.Price >= parametrs.minPrice &&
                                      book.Price <= parametrs.maxPrice);

        var dtos = list.Select(book => _mapper.Map<BookDto>(book)).ToList();

        // Order by title
        if (parametrs.orderByTitle)
        {
            dtos = dtos.OrderBy(book => book.Title).ToList();
        }
        else
        {
            dtos = dtos.OrderByDescending(book => book.Title).ToList();
        }

        PagedList<BookDto> pagedList = new(dtos, dtos.Count,
                                                          parametrs.PageNumber, parametrs.pageSize);

        return pagedList.ToPagedList(dtos, parametrs.PageSize, parametrs.PageNumber);
    }

    public async Task<BookDto> GetBookByIdAsync(int id)
    {
        var book = await _unitOfWork.BookInterface.GetByIdAsync(id);
        return _mapper.Map<BookDto>(book);
    }

    public async Task<List<BookDto>> GetBooksAsync()
    {
        var list = await _unitOfWork.BookInterface.GetBooksWithCategoryAsync();
        return list.Select(book => _mapper.Map<BookDto>(book)).ToList();
    }

    public async Task<PagedList<BookDto>> GetPagetBooks(int pageSize, int pageNumber)
    {
        var list = await _unitOfWork.BookInterface.GetAllAsync();
        var dtos = list.Select(c => _mapper.Map<BookDto>(c))
                       .ToList();
        PagedList<BookDto> pagedList = new(dtos,
                                                          dtos.Count(),
                                                          pageNumber,
                                                          pageSize);
        return pagedList.ToPagedList(dtos, pageSize, pageNumber);
    }

    public async Task UpdateBookAsync(BookDto BookDto)
    {
        var book = _mapper.Map<Book>(BookDto);
        _unitOfWork.BookInterface.Update(book);
        await _unitOfWork.SaveAsync();
    }
}
