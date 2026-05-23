using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class SiteManagementQuery
{
    [Authorize]
    public IReadOnlyCollection<Site> GetSites([Service] ISiteService siteService) =>
        siteService.GetSites();

    [Authorize]
    public SitePage GetSitesPage(
        int pageNumber,
        int pageSize,
        string? search,
        [Service] ISiteService siteService) =>
        siteService.GetSitesPage(pageNumber, pageSize, search);

    [Authorize]
    public Site? GetSite(int id, [Service] ISiteService siteService) =>
        siteService.GetSite(id);
}
