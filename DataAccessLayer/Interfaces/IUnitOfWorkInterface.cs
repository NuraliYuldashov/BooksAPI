namespace DataAccessLayer.Interfaces;

public interface IUnitOfWorkInterface : IDisposable
{
    ICategoryInterface CategoryInterface { get; }
    IBookInterface BookInterface { get; }
    Task SaveAsync();
}
