using HRLConstructionManagerAPI.Entities;

namespace HRLConstructionManagerAPI.Repositories;

public interface ISiteRepository
{
    IReadOnlyCollection<Site> GetAll();

    (IReadOnlyCollection<Site> Items, int TotalCount) GetPaged(
        int pageNumber,
        int pageSize,
        string? search);

    Site? GetById(int id);

    Site Add(Site site);

    Site? Update(Site site);

    bool Delete(int id);
}
