using HRLConstructionManagerAPI.Entities;
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
    public Site? GetSite(int id, [Service] ISiteService siteService) =>
        siteService.GetSite(id);
}
