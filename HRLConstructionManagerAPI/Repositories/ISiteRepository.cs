using HRLConstructionManagerAPI.Entities;

namespace HRLConstructionManagerAPI.Repositories;

public interface ISiteRepository
{
    IReadOnlyCollection<Site> GetAll();

    Site? GetById(int id);

    Site Add(Site site);

    Site? Update(Site site);

    bool Delete(int id);
}
