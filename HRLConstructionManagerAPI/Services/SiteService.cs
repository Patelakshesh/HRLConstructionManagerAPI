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
        ValidateCreateInput(input);

        var site = new Site
        {
            SiteName = input.SiteName.Trim(),
            Address = input.Address.Trim(),
            City = string.IsNullOrWhiteSpace(input.City) ? null : input.City.Trim(),
            State = string.IsNullOrWhiteSpace(input.State) ? null : input.State.Trim(),
            ContactPerson = string.IsNullOrWhiteSpace(input.ContactPerson) ? null : input.ContactPerson.Trim(),
            ContactNumber = string.IsNullOrWhiteSpace(input.ContactNumber) ? null : input.ContactNumber.Trim(),
            StartDate = input.StartDate,
            EndDate = input.EndDate,
            Enable = input.Enable,
            CreatedBy = input.CreatedBy
        };

        return siteRepository.Add(site);
    }

    public Site? UpdateSite(UpdateSiteInput input)
    {
        ValidateUpdateInput(input);

        var site = new Site
        {
            Id = input.Id,
            SiteName = input.SiteName.Trim(),
            Address = input.Address.Trim(),
            City = string.IsNullOrWhiteSpace(input.City) ? null : input.City.Trim(),
            State = string.IsNullOrWhiteSpace(input.State) ? null : input.State.Trim(),
            ContactPerson = string.IsNullOrWhiteSpace(input.ContactPerson) ? null : input.ContactPerson.Trim(),
            ContactNumber = string.IsNullOrWhiteSpace(input.ContactNumber) ? null : input.ContactNumber.Trim(),
            StartDate = input.StartDate,
            EndDate = input.EndDate,
            Enable = input.Enable,
            ModifiedBy = input.ModifiedBy
        };

        return siteRepository.Update(site);
    }

    public bool DeleteSite(int id) => siteRepository.Delete(id);

    private static void ValidateCreateInput(CreateSiteInput input)
    {
        if (string.IsNullOrWhiteSpace(input.SiteName))
        {
            throw new GraphQLException("Site name is required.");
        }

        if (input.SiteName.Trim().Length > 200)
        {
            throw new GraphQLException("Site name must not exceed 200 characters.");
        }

        if (string.IsNullOrWhiteSpace(input.Address))
        {
            throw new GraphQLException("Address is required.");
        }

        if (!string.IsNullOrWhiteSpace(input.ContactNumber) && !IsValidContactNumber(input.ContactNumber))
        {
            throw new GraphQLException("Contact number must be a valid 10-digit number.");
        }

        if (input.StartDate.HasValue && input.EndDate.HasValue && input.EndDate < input.StartDate)
        {
            throw new GraphQLException("End date cannot be before the start date.");
        }
    }

    private static void ValidateUpdateInput(UpdateSiteInput input)
    {
        if (input.Id <= 0)
        {
            throw new GraphQLException("Site id is required.");
        }

        if (string.IsNullOrWhiteSpace(input.SiteName))
        {
            throw new GraphQLException("Site name is required.");
        }

        if (input.SiteName.Trim().Length > 200)
        {
            throw new GraphQLException("Site name must not exceed 200 characters.");
        }

        if (string.IsNullOrWhiteSpace(input.Address))
        {
            throw new GraphQLException("Address is required.");
        }

        if (!string.IsNullOrWhiteSpace(input.ContactNumber) && !IsValidContactNumber(input.ContactNumber))
        {
            throw new GraphQLException("Contact number must be a valid 10-digit number.");
        }

        if (input.StartDate.HasValue && input.EndDate.HasValue && input.EndDate < input.StartDate)
        {
            throw new GraphQLException("End date cannot be before the start date.");
        }
    }

    private static bool IsValidContactNumber(string number) =>
        System.Text.RegularExpressions.Regex.IsMatch(number.Trim(), @"^\d{10}$");

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
