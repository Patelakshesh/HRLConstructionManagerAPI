using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;

namespace HRLConstructionManagerAPI.Services;

public sealed partial class ContractorService(IContractorRepository contractorRepository) : IContractorService
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public IReadOnlyCollection<Contractor> GetContractors() => contractorRepository.GetAll();

    public ContractorPage GetContractorsPage(int pageNumber, int pageSize, string? search)
    {
        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1
            ? DefaultPageSize
            : Math.Min(pageSize, MaxPageSize);
        var normalizedSearch = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

        var (items, totalCount) = contractorRepository.GetPaged(
            normalizedPageNumber,
            normalizedPageSize,
            normalizedSearch);

        var totalPages = normalizedPageSize == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)normalizedPageSize);

        return new ContractorPage(
            items.Select(ToDto).ToArray(),
            totalCount,
            normalizedPageNumber,
            normalizedPageSize,
            totalPages);
    }

    public Contractor? GetContractor(int id) => contractorRepository.GetById(id);

    public Contractor CreateContractor(CreateContractorInput input)
    {
        var contractor = BuildContractor(input.CompanyName, input.ContactPerson, input.Email, input.Phone, input.AssignedSites, input.Enable);
        contractor.CreatedBy = input.CreatedBy;

        return contractorRepository.Add(contractor);
    }

    public Contractor? UpdateContractor(UpdateContractorInput input)
    {
        var contractor = BuildContractor(input.CompanyName, input.ContactPerson, input.Email, input.Phone, input.AssignedSites, input.Enable);
        contractor.Id = input.Id;
        contractor.ModifiedBy = input.ModifiedBy;

        return contractorRepository.Update(contractor);
    }

    private static Contractor BuildContractor(
        string companyName, string contactPerson, string email,
        string phone, string? assignedSites, bool enable) =>
        new()
        {
            CompanyName = companyName.Trim(),
            ContactPerson = contactPerson.Trim(),
            Email = email.Trim(),
            Phone = phone.Trim(),
            AssignedSites = assignedSites?.Trim(),
            Enable = enable
        };

    public bool DeleteContractor(int id) => contractorRepository.Delete(id);


    private static ContractorDto ToDto(Contractor contractor) =>
        new(
            contractor.Id,
            contractor.CompanyName,
            contractor.ContactPerson,
            contractor.Email,
            contractor.Phone,
            contractor.AssignedSites,
            contractor.Enable,
            contractor.CreatedOn,
            contractor.CreatedBy,
            contractor.ModifiedOn,
            contractor.ModifiedBy);
}
