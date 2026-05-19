using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;

namespace HRLConstructionManagerAPI.Services;

public sealed class SiteService(ISiteRepository siteRepository) : ISiteService
{
    public IReadOnlyCollection<Site> GetSites() => siteRepository.GetAll();

    public Site? GetSite(int id) => siteRepository.GetById(id);

    public Site CreateSite(CreateSiteInput input)
    {
        var site = new Site
        {
            SiteName = input.SiteName,
            Address = input.Address,
            City = input.City,
            State = input.State,
            ContactPerson = input.ContactPerson,
            ContactNumber = input.ContactNumber,
            StartDate = input.StartDate,
            EndDate = input.EndDate,
            Enable = input.Enable,
            CreatedBy = input.CreatedBy
        };

        return siteRepository.Add(site);
    }

    public Site? UpdateSite(UpdateSiteInput input)
    {
        var site = new Site
        {
            Id = input.Id,
            SiteName = input.SiteName,
            Address = input.Address,
            City = input.City,
            State = input.State,
            ContactPerson = input.ContactPerson,
            ContactNumber = input.ContactNumber,
            StartDate = input.StartDate,
            EndDate = input.EndDate,
            Enable = input.Enable,
            ModifiedBy = input.ModifiedBy
        };

        return siteRepository.Update(site);
    }

    public bool DeleteSite(int id) => siteRepository.Delete(id);
}
