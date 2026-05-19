using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class SiteManagementMutation
{
    [Authorize]
    public Site CreateSite(CreateSiteInput input, [Service] ISiteService siteService) =>
        siteService.CreateSite(input);

    [Authorize]
    public Site? UpdateSite(UpdateSiteInput input, [Service] ISiteService siteService) =>
        siteService.UpdateSite(input);

    [Authorize]
    public bool DeleteSite(int id, [Service] ISiteService siteService) =>
        siteService.DeleteSite(id);
}
