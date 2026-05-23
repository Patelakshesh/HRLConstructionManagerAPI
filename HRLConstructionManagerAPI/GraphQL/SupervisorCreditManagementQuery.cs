using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class SupervisorCreditManagementQuery
{
    [Authorize]
    public IReadOnlyCollection<SupervisorCredit> GetSupervisorCredits([Service] ISupervisorCreditService creditService) =>
        creditService.GetSupervisorCredits();

    [Authorize]
    public SupervisorCreditPage GetSupervisorCreditsPage(
        int pageNumber,
        int pageSize,
        string? search,
        string? supervisorName,
        string? paymentMode,
        [Service] ISupervisorCreditService creditService) =>
        creditService.GetSupervisorCreditsPage(pageNumber, pageSize, search, supervisorName, paymentMode);

    [Authorize]
    public SupervisorCredit? GetSupervisorCredit(int id, [Service] ISupervisorCreditService creditService) =>
        creditService.GetSupervisorCredit(id);
}
