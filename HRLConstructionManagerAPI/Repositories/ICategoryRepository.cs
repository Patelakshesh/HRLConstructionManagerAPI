using HRLConstructionManagerAPI.Entities;

namespace HRLConstructionManagerAPI.Repositories;

public interface ICategoryRepository
{
    IReadOnlyCollection<Category> GetAll();

    (IReadOnlyCollection<Category> Items, int TotalCount) GetPaged(int pageNumber, int pageSize, string? search);

    Category? GetById(int id);

    Category Add(Category category);

    Category? Update(Category category);

    bool Delete(int id);
}
