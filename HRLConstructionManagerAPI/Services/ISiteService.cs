using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;

namespace HRLConstructionManagerAPI.Services;

public interface ISiteService
{
    IReadOnlyCollection<Site> GetSites();

    SitePage GetSitesPage(int pageNumber, int pageSize, string? search);

    Site? GetSite(int id);

    Site CreateSite(CreateSiteInput input);

    Site? UpdateSite(UpdateSiteInput input);

    bool DeleteSite(int id);
}
