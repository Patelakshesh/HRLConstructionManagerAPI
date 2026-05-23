using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class CategoryManagementMutation
{
    [Authorize]
    public Category CreateCategory(CreateCategoryInput input, [Service] ICategoryService categoryService) =>
        categoryService.CreateCategory(input);

    [Authorize]
    public Category? UpdateCategory(UpdateCategoryInput input, [Service] ICategoryService categoryService) =>
        categoryService.UpdateCategory(input);

    [Authorize]
    public bool DeleteCategory(int id, [Service] ICategoryService categoryService) =>
        categoryService.DeleteCategory(id);
}
