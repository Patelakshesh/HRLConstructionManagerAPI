using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class ContractorManagementMutation
{
    [Authorize]
    public Contractor CreateContractor(CreateContractorInput input, [Service] IContractorService contractorService) =>
        contractorService.CreateContractor(input);

    [Authorize]
    public Contractor? UpdateContractor(UpdateContractorInput input, [Service] IContractorService contractorService) =>
        contractorService.UpdateContractor(input);

    [Authorize]
    public bool DeleteContractor(int id, [Service] IContractorService contractorService) =>
        contractorService.DeleteContractor(id);
}
