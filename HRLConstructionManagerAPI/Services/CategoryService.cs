using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;

namespace HRLConstructionManagerAPI.Services;

public sealed class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public IReadOnlyCollection<Category> GetCategories() => categoryRepository.GetAll();

    public CategoryPage GetCategoriesPage(int pageNumber, int pageSize, string? search)
    {
        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1
            ? DefaultPageSize
            : Math.Min(pageSize, MaxPageSize);
        var normalizedSearch = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

        var (items, totalCount) = categoryRepository.GetPaged(
            normalizedPageNumber,
            normalizedPageSize,
            normalizedSearch);

        var totalPages = normalizedPageSize == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)normalizedPageSize);

        return new CategoryPage(
            items.Select(ToDto).ToArray(),
            totalCount,
            normalizedPageNumber,
            normalizedPageSize,
            totalPages);
    }

    public Category? GetCategory(int id) => categoryRepository.GetById(id);

    public Category CreateCategory(CreateCategoryInput input)
    {
        var category = BuildCategory(input.Name, input.Description, input.Enable);
        category.CreatedBy = input.CreatedBy;

        return categoryRepository.Add(category);
    }

    public Category? UpdateCategory(UpdateCategoryInput input)
    {
        var category = BuildCategory(input.Name, input.Description, input.Enable);
        category.Id = input.Id;
        category.ModifiedBy = input.ModifiedBy;

        return categoryRepository.Update(category);
    }

    private static Category BuildCategory(string name, string? description, bool enable) =>
        new()
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            Enable = enable
        };

    public bool DeleteCategory(int id) => categoryRepository.Delete(id);

    private static CategoryDto ToDto(Category category) =>
        new(
            category.Id,
            category.Name,
            category.Description,
            category.Enable,
            category.CreatedOn,
            category.CreatedBy,
            category.ModifiedOn,
            category.ModifiedBy);
}
