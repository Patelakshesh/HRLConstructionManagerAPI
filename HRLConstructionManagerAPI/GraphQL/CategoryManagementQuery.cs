using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class CategoryManagementQuery
{
    [Authorize]
    public IReadOnlyCollection<Category> GetCategories([Service] ICategoryService categoryService) =>
        categoryService.GetCategories();

    [Authorize]
    public CategoryPage GetCategoriesPage(
        int pageNumber,
        int pageSize,
        string? search,
        [Service] ICategoryService categoryService) =>
        categoryService.GetCategoriesPage(pageNumber, pageSize, search);

    [Authorize]
    public Category? GetCategory(int id, [Service] ICategoryService categoryService) =>
        categoryService.GetCategory(id);
}
