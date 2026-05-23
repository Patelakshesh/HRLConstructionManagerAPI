using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;

namespace HRLConstructionManagerAPI.Services;

public interface IContractorService
{
    IReadOnlyCollection<Contractor> GetContractors();

    ContractorPage GetContractorsPage(int pageNumber, int pageSize, string? search);

    Contractor? GetContractor(int id);

    Contractor CreateContractor(CreateContractorInput input);

    Contractor? UpdateContractor(UpdateContractorInput input);

    bool DeleteContractor(int id);
}
