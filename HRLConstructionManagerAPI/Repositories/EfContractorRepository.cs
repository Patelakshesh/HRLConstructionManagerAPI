using HRLConstructionManagerAPI.Data;
using HRLConstructionManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRLConstructionManagerAPI.Repositories;

public sealed class EfContractorRepository(AppDbContext dbContext) : IContractorRepository
{
    public IReadOnlyCollection<Contractor> GetAll() =>
        dbContext.Contractors
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToArray();

    public (IReadOnlyCollection<Contractor> Items, int TotalCount) GetPaged(
        int pageNumber,
        int pageSize,
        string? search)
    {
        var query = dbContext.Contractors.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(c =>
                c.ContractorName.Contains(term) ||
                c.Email.Contains(term) ||
                c.Phone.Contains(term));
        }

        var totalCount = query.Count();
        var items = query
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArray();

        return (items, totalCount);
    }

    public Contractor? GetById(int id) =>
        dbContext.Contractors
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == id);

    public Contractor Add(Contractor contractor)
    {
        dbContext.Contractors.Add(contractor);
        dbContext.SaveChanges();
        return contractor;
    }

    public Contractor? Update(Contractor contractor)
    {
        var existing = dbContext.Contractors.FirstOrDefault(c => c.Id == contractor.Id);
        if (existing is null) return null;

        existing.ContractorName = contractor.ContractorName;
        existing.Email = contractor.Email;
        existing.Phone = contractor.Phone;
        existing.AssignedSites = contractor.AssignedSites;
        existing.Enable = contractor.Enable;
        existing.ModifiedOn = DateTime.UtcNow;
        existing.ModifiedBy = contractor.ModifiedBy;

        dbContext.SaveChanges();
        return existing;
    }

    public bool Delete(int id)
    {
        var contractor = dbContext.Contractors.FirstOrDefault(c => c.Id == id);
        if (contractor is null) return false;

        dbContext.Contractors.Remove(contractor);
        dbContext.SaveChanges();
        return true;
    }
}
