using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;

namespace HRLConstructionManagerAPI.Services;

public sealed class SiteService(ISiteRepository siteRepository) : ISiteService
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public IReadOnlyCollection<Site> GetSites() => siteRepository.GetAll();

    public SitePage GetSitesPage(int pageNumber, int pageSize, string? search)
    {
        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1
            ? DefaultPageSize
            : Math.Min(pageSize, MaxPageSize);
        var normalizedSearch = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

        var (items, totalCount) = siteRepository.GetPaged(
            normalizedPageNumber,
            normalizedPageSize,
            normalizedSearch);

        var totalPages = normalizedPageSize == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)normalizedPageSize);

        return new SitePage(
            items.Select(ToDto).ToArray(),
            totalCount,
            normalizedPageNumber,
            normalizedPageSize,
            totalPages);
    }

    public Site? GetSite(int id) => siteRepository.GetById(id);

    public Site CreateSite(CreateSiteInput input)
    {
        ValidateDates(input.StartDate, input.EndDate);

        var site = BuildSite(input.SiteName, input.Address, input.City, input.State,
            input.ContactPerson, input.ContactNumber, input.StartDate, input.EndDate, input.Enable);
        site.CreatedBy = input.CreatedBy;

        return siteRepository.Add(site);
    }

    public Site? UpdateSite(UpdateSiteInput input)
    {
        ValidateDates(input.StartDate, input.EndDate);

        var site = BuildSite(input.SiteName, input.Address, input.City, input.State,
            input.ContactPerson, input.ContactNumber, input.StartDate, input.EndDate, input.Enable);
        site.Id = input.Id;
        site.ModifiedBy = input.ModifiedBy;

        return siteRepository.Update(site);
    }

    private static Site BuildSite(
        string siteName, string address, string? city, string? state,
        string? contactPerson, string? contactNumber,
        DateTime? startDate, DateTime? endDate, bool enable) =>
        new()
        {
            SiteName = siteName.Trim(),
            Address = address.Trim(),
            City = string.IsNullOrWhiteSpace(city) ? null : city.Trim(),
            State = string.IsNullOrWhiteSpace(state) ? null : state.Trim(),
            ContactPerson = string.IsNullOrWhiteSpace(contactPerson) ? null : contactPerson.Trim(),
            ContactNumber = string.IsNullOrWhiteSpace(contactNumber) ? null : contactNumber.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            Enable = enable
        };

    public bool DeleteSite(int id) => siteRepository.Delete(id);

    private static void ValidateDates(DateTime? startDate, DateTime? endDate)
    {
        if (startDate.HasValue && endDate.HasValue && endDate < startDate)
        {
            throw new GraphQLException("End date cannot be before the start date.");
        }
    }


    private static SiteDto ToDto(Site site) =>
        new(
            site.Id,
            site.SiteName,
            site.Address,
            site.City,
            site.State,
            site.ContactPerson,
            site.ContactNumber,
            site.StartDate,
            site.EndDate,
            site.Enable,
            site.CreatedOn,
            site.CreatedBy,
            site.ModifiedOn,
            site.ModifiedBy);
}
