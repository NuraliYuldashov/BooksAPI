using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories;

public class UnitOfWork : IUnitOfWorkInterface
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext,
                      ICategoryInterface categoryInterface,
                      IBookInterface bookInterface)
    {
        _dbContext = dbContext;
        CategoryInterface = categoryInterface;
        BookInterface = bookInterface;
    }

    public ICategoryInterface CategoryInterface { get; }

    public IBookInterface BookInterface { get; }

    public void Dispose()
        => GC.SuppressFinalize(this);

    public async Task SaveAsync()
        => await _dbContext.SaveChangesAsync();
}
