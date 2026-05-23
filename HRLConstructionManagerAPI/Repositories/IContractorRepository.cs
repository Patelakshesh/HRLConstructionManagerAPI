using HRLConstructionManagerAPI.Entities;

namespace HRLConstructionManagerAPI.Repositories;

public interface IContractorRepository
{
    IReadOnlyCollection<Contractor> GetAll();

    (IReadOnlyCollection<Contractor> Items, int TotalCount) GetPaged(int pageNumber, int pageSize, string? search);

    Contractor? GetById(int id);

    Contractor Add(Contractor contractor);

    Contractor? Update(Contractor contractor);

    bool Delete(int id);
}
