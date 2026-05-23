using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HRLConstructionManagerAPI.Repositories;

public sealed class EfSiteRepository(AppDbContext dbContext) : ISiteRepository
{
    public IReadOnlyCollection<Site> GetAll() =>
        dbContext.Sites
            .AsNoTracking()
            .OrderBy(site => site.Id)
            .ToArray();

    public (IReadOnlyCollection<Site> Items, int TotalCount) GetPaged(
        int pageNumber,
        int pageSize,
        string? search)
    {
        var query = dbContext.Sites.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(site =>
                site.SiteName.Contains(term) ||
                site.Address.Contains(term) ||
                (site.City != null && site.City.Contains(term)) ||
                (site.ContactPerson != null && site.ContactPerson.Contains(term)));
        }

        var totalCount = query.Count();
        var items = query
            .OrderBy(site => site.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArray();

        return (items, totalCount);
    }

    public Site? GetById(int id) =>
        dbContext.Sites
            .AsNoTracking()
            .FirstOrDefault(site => site.Id == id);

    public Site Add(Site site)
    {
        dbContext.Sites.Add(site);
        dbContext.SaveChanges();

        return site;
    }

    public Site? Update(Site site)
    {
        var existingSite = dbContext.Sites.FirstOrDefault(currentSite => currentSite.Id == site.Id);
        if (existingSite is null)
        {
            return null;
        }

        existingSite.SiteName = site.SiteName;
        existingSite.Address = site.Address;
        existingSite.City = site.City;
        existingSite.State = site.State;
        existingSite.ContactPerson = site.ContactPerson;
        existingSite.ContactNumber = site.ContactNumber;
        existingSite.StartDate = site.StartDate;
        existingSite.EndDate = site.EndDate;
        existingSite.Enable = site.Enable;
        existingSite.ModifiedOn = DateTime.UtcNow;
        existingSite.ModifiedBy = site.ModifiedBy;

        dbContext.SaveChanges();

        return existingSite;
    }

    public bool Delete(int id)
    {
        var site = dbContext.Sites.FirstOrDefault(currentSite => currentSite.Id == id);
        if (site is null)
        {
            return false;
        }

        dbContext.Sites.Remove(site);
        dbContext.SaveChanges();

        return true;
    }
}
