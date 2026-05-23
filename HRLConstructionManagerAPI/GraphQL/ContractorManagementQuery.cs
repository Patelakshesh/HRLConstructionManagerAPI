using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class ContractorManagementQuery
{
    [Authorize]
    public IReadOnlyCollection<Contractor> GetContractors([Service] IContractorService contractorService) =>
        contractorService.GetContractors();

    [Authorize]
    public ContractorPage GetContractorsPage(
        int pageNumber,
        int pageSize,
        string? search,
        [Service] IContractorService contractorService) =>
        contractorService.GetContractorsPage(pageNumber, pageSize, search);

    [Authorize]
    public Contractor? GetContractor(int id, [Service] IContractorService contractorService) =>
        contractorService.GetContractor(id);
}
