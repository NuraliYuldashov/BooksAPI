using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces;

public interface ICategoryInterface : IRepositoryInterface<Category>
{
    Task<IEnumerable<Category>> GetAllCategoriesWithBoksAsyc();
}
