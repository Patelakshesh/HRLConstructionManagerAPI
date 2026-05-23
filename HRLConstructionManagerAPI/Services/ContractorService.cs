using System.Net.Mail;
using System.Text.RegularExpressions;
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
        ValidateInput(input.CompanyName, input.ContactPerson, input.Email, input.Phone);

        var contractor = new Contractor
        {
            CompanyName = input.CompanyName.Trim(),
            ContactPerson = input.ContactPerson.Trim(),
            Email = input.Email.Trim(),
            Phone = input.Phone.Trim(),
            AssignedSites = input.AssignedSites?.Trim(),
            Enable = input.Enable,
            CreatedBy = input.CreatedBy
        };

        return contractorRepository.Add(contractor);
    }

    public Contractor? UpdateContractor(UpdateContractorInput input)
    {
        ValidateInput(input.CompanyName, input.ContactPerson, input.Email, input.Phone);

        var contractor = new Contractor
        {
            Id = input.Id,
            CompanyName = input.CompanyName.Trim(),
            ContactPerson = input.ContactPerson.Trim(),
            Email = input.Email.Trim(),
            Phone = input.Phone.Trim(),
            AssignedSites = input.AssignedSites?.Trim(),
            Enable = input.Enable,
            ModifiedBy = input.ModifiedBy
        };

        return contractorRepository.Update(contractor);
    }

    public bool DeleteContractor(int id) => contractorRepository.Delete(id);

    private static void ValidateInput(string companyName, string contactPerson, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            throw new ArgumentException("Company name is required.");
        }

        if (string.IsNullOrWhiteSpace(contactPerson))
        {
            throw new ArgumentException("Contact person is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.");
        }
        else
        {
            try
            {
                var mailAddress = new MailAddress(email);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email format.");
            }
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Phone number is required.");
        }
        else if (!PhoneRegex().IsMatch(phone))
        {
            throw new ArgumentException("Phone number must be exactly 10 digits.");
        }
    }

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

    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex PhoneRegex();
}
