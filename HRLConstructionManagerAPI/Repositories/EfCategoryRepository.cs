using HRLConstructionManagerAPI.Data;
using HRLConstructionManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRLConstructionManagerAPI.Repositories;

public sealed class EfCategoryRepository(AppDbContext dbContext) : ICategoryRepository
{
    public IReadOnlyCollection<Category> GetAll() =>
        dbContext.Categories
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToArray();

    public (IReadOnlyCollection<Category> Items, int TotalCount) GetPaged(
        int pageNumber,
        int pageSize,
        string? search)
    {
        var query = dbContext.Categories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(c =>
                c.Name.Contains(term) ||
                (c.Description != null && c.Description.Contains(term)));
        }

        var totalCount = query.Count();
        var items = query
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArray();

        return (items, totalCount);
    }

    public Category? GetById(int id) =>
        dbContext.Categories
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == id);

    public Category Add(Category category)
    {
        dbContext.Categories.Add(category);
        dbContext.SaveChanges();
        return category;
    }

    public Category? Update(Category category)
    {
        var existing = dbContext.Categories.FirstOrDefault(c => c.Id == category.Id);
        if (existing is null) return null;

        existing.Name = category.Name;
        existing.Description = category.Description;
        existing.Enable = category.Enable;
        existing.ModifiedOn = DateTime.UtcNow;
        existing.ModifiedBy = category.ModifiedBy;

        dbContext.SaveChanges();
        return existing;
    }

    public bool Delete(int id)
    {
        var category = dbContext.Categories.FirstOrDefault(c => c.Id == id);
        if (category is null) return false;

        dbContext.Categories.Remove(category);
        dbContext.SaveChanges();
        return true;
    }
}
