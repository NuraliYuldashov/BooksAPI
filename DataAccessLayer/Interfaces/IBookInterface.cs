using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces;

public interface IBookInterface : IRepositoryInterface<Book>
{
    Task<IEnumerable<Book>> GetBooksWithCategoryAsync();
}

