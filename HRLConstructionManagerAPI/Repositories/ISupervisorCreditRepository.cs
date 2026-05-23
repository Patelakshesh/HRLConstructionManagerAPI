using HRLConstructionManagerAPI.Entities;

namespace HRLConstructionManagerAPI.Repositories;

public interface ISupervisorCreditRepository
{
    IReadOnlyCollection<SupervisorCredit> GetAll();

    (IReadOnlyCollection<SupervisorCredit> Items, int TotalCount) GetPaged(
        int pageNumber,
        int pageSize,
        string? search,
        string? supervisorName,
        string? paymentMode);

    SupervisorCredit? GetById(int id);

    SupervisorCredit Add(SupervisorCredit credit);

    SupervisorCredit? Update(SupervisorCredit credit);

    bool Delete(int id);
}
