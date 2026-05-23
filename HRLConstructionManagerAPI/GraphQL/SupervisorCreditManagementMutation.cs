using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class SupervisorCreditManagementMutation
{
    [Authorize]
    public SupervisorCredit CreateSupervisorCredit(CreateSupervisorCreditInput input, [Service] ISupervisorCreditService creditService) =>
        creditService.CreateSupervisorCredit(input);

    [Authorize]
    public SupervisorCredit? UpdateSupervisorCredit(UpdateSupervisorCreditInput input, [Service] ISupervisorCreditService creditService) =>
        creditService.UpdateSupervisorCredit(input);

    [Authorize]
    public bool DeleteSupervisorCredit(int id, [Service] ISupervisorCreditService creditService) =>
        creditService.DeleteSupervisorCredit(id);
}
