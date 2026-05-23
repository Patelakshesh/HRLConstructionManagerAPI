using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;

namespace HRLConstructionManagerAPI.Services;

public interface ICategoryService
{
    IReadOnlyCollection<Category> GetCategories();

    CategoryPage GetCategoriesPage(int pageNumber, int pageSize, string? search);

    Category? GetCategory(int id);

    Category CreateCategory(CreateCategoryInput input);

    Category? UpdateCategory(UpdateCategoryInput input);

    bool DeleteCategory(int id);
}
