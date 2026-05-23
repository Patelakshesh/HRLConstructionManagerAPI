using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;

namespace HRLConstructionManagerAPI.Services;

public interface ISupervisorCreditService
{
    IReadOnlyCollection<SupervisorCredit> GetSupervisorCredits();

    SupervisorCreditPage GetSupervisorCreditsPage(
        int pageNumber, 
        int pageSize, 
        string? search, 
        string? supervisorName, 
        string? paymentMode);

    SupervisorCredit? GetSupervisorCredit(int id);

    SupervisorCredit CreateSupervisorCredit(CreateSupervisorCreditInput input);

    SupervisorCredit? UpdateSupervisorCredit(UpdateSupervisorCreditInput input);

    bool DeleteSupervisorCredit(int id);
}
